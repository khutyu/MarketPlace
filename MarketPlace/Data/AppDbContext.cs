
using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Category>().ToTable("Category");
        //    modelBuilder.Entity<Product>().ToTable("Product");
        //    modelBuilder.Entity<Chat>().ToTable("Chat");
        //    modelBuilder.Entity<Message>().ToTable("Message");
        //    modelBuilder.Entity<Comment>().ToTable("Comment");
        //    modelBuilder.Entity<Address>().ToTable("Address");
        //    modelBuilder.Entity<User>().ToTable("User");
        //}
    }
}
