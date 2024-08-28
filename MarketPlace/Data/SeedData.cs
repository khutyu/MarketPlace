using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Data
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if(!context.Categories.Any())
            {
                context.Categories.AddRange(
                     new Category { CategoryName = "Electronics" },
                     new Category { CategoryName="Toiletries"},
                     new Category { CategoryName="Furniture"},
                     new Category { CategoryName="Clothing"},
                     new Category { CategoryName="Food"},
                     new Category { CategoryName="Education"},
                     new Category { CategoryName="Services"},
                     new Category { CategoryName="Other"}
                );
            }

            context.SaveChanges();
        }
    }
}
