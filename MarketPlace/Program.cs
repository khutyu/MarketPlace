
using MarketPlace.Models;
using MarketPlace.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MarketPlace.Hubs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

//Adding signalR services 
builder.Services.AddSignalR();

//Database Option 1: SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




builder.Services.AddIdentity<User, IdentityRole>(opts => {
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireDigit = false;
    opts.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>();


var app = builder.Build();

//Configure middleware
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=chat}/{action=Index}/{id?}");

//SeedData.EnsureEntityPopulated(app);
//SeedIdentityData.EnsureIdentityPopulated(app);
// mapping existing hubs
app.MapHub<ChatHub>("/chatHub");
app.Run();
