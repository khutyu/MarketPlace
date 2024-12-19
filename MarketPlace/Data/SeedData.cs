using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class SeedData
{
    public class CustomUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public Gender gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsAgreedToTerms { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

    private static readonly List<CustomUserModel> SeedUsers = new()
    {
        new CustomUserModel
        {   FirstName = "Meggie",
            LastName = "Griffin",
            IsAgreedToTerms = true,
            DateOfBirth = DateTime.Now,
            gender = Gender.Female,
            Username = "Maggie",
            Email = "maggie@example.com",
            Role = "Buyer"
        },
        new CustomUserModel
        {
            FirstName = "Meggie",
            LastName = "Griffin",
            IsAgreedToTerms = true,
            DateOfBirth = DateTime.Now,
            gender = Gender.Female,
            Username = "Admin",
            Email = "admin@example.com",
            Role = "Administrator"
        }
    };

    public static void PopulateDatabase(IApplicationBuilder app)
    {
        AppDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
        EnsureIdentityPopulatedAsync(app).Wait();
    }

    public static async Task EnsureIdentityPopulatedAsync(IApplicationBuilder app)
    {
        try
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                // Ensure roles are created
                var roles = SeedUsers.Select(u => u.Role).Distinct();
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to create role {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                // Ensure users are created and assigned to roles
                foreach (var seedUser in SeedUsers)
                {
                    var user = await userManager.FindByNameAsync(seedUser.Username);
                    if (user == null)
                    {
                        user = new User
                        {
                            IsAgreedToTerms = true,
                            Address = new Address(),
                            DateOfBirth = seedUser.DateOfBirth,
                            Gender = seedUser.gender,
                            LastName = seedUser.LastName,
                            FirstName = seedUser.FirstName,
                            UserName = seedUser.Username,
                            Email = seedUser.Email,
                            EmailConfirmed = true
                        };

                        var result = await userManager.CreateAsync(user, "Password123!");
                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to create user {seedUser.Username}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }

                    if (!await userManager.IsInRoleAsync(user, seedUser.Role))
                    {
                        var result = await userManager.AddToRoleAsync(user, seedUser.Role);
                        if (!result.Succeeded)
                        {
                            throw new Exception($"Failed to add user {seedUser.Username} to role {seedUser.Role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw; // Rethrow to ensure the error is not silently swallowed
        }
    }

    public static async Task EnsureEntityPopulatedAsync(IApplicationBuilder app) // Corrected method name
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Only seed categories if none exist
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { CategoryName = "Electronics" },
                    new Category { CategoryName = "Toiletries" },
                    new Category { CategoryName = "Furniture" },
                    new Category { CategoryName = "Clothing" },
                    new Category { CategoryName = "Food" },
                    new Category { CategoryName = "Education" },
                    new Category { CategoryName = "Services" },
                    new Category { CategoryName = "Other" }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
