using MarketPlace.Models;
using MarketPlace.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarketPlace.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddTransient<IEmailService, EmailService>();

//Adding signalR services 
builder.Services.AddSignalR();

// Database Contexts
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Configuration
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
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
    pattern: "{controller=chat}/{action=Index}/{id?}");

// Seed roles and users
SeedData.PopulateDatabase(app);
// mapping existing hubs
app.MapHub<ChatHub>("/chatHub");
app.Run();
