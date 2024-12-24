using MarketPlace.Chat.Hubs;
using MarketPlace.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

    // Configure services
    builder.Services.AddControllersWithViews();
    builder.Services.AddSignalR();
    builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// Add Identity services
builder.Services.AddIdentity<MarketPlace.Shared.Models.User, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

    // SQL Server Database
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    var app = builder.Build();

    // Configure middleware
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication(); // Ensure authentication middleware is added
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
            name: "marketplace.chat",
            pattern: "Marketplace.Chat/{controller=Home}/{action=Index}/{id?}");

    app.MapHub<ChatHub>("/chatHub");

    app.Run();

