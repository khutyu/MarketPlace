using MarketPlace.Shared.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Shared
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Product>().ToTable("Product");

            // Configure Message entity
            modelBuilder.Entity<Message>()
                .Property(m => m.Content)
                .IsRequired();

            modelBuilder.Entity<Message>()
                .Property(m => m.Timestamp)
                .HasDefaultValueSql("GETDATE()");

            // Define relationships
            modelBuilder.Entity<Message>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Chat>().ToTable("Chat");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<Notification>().ToTable("Notification");
        }
    }
}
