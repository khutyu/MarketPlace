using MarketPlace.Models;
using MarketPlace.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Contexts
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

// Identity Configuration
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false; // Adjust as needed
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true; // Require at least one digit
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppIdentityDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Set the login path
});
var app = builder.Build();

// Middleware
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

// Route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed roles and users
SeedData.PopulateDatabase(app);

app.Run();
