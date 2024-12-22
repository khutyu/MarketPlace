using letschat.Models;
using Microsoft.EntityFrameworkCore;

namespace letschat.Data
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

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Id = "user1",
                        Username = "Alice",
                        ProfilePicture = "/images/alice.jpeg",
                        IsOnline = true,
                        LastActive = DateTime.UtcNow
                    },
                    new User
                    {
                        Id = "user2",
                        Username = "Bob",
                        ProfilePicture = "/images/bob.jpeg",
                        IsOnline = false,
                        LastActive = DateTime.UtcNow.AddMinutes(-30)
                    },
                    new User
                    {
                        Id = "user3",
                        Username = "James",
                        ProfilePicture = "/images/bob.jpeg",
                        IsOnline = false,
                        LastActive = DateTime.UtcNow.AddMinutes(-30)
                    }
                    );
            }
            if (!context.Messages.Any())
            {
                context.Messages.AddRange(
                     new Message
                     {
                
                         SenderId = "user1",
                         ReceiverId = "user2",
                         Content = "Hi Bob!",
                         Timestamp = DateTime.UtcNow,
                         IsRead = false
                     },
                     new Message
                     {
                      
                         SenderId = "user2",
                         ReceiverId = "user1",
                         Content = "Hey Alice! How are you?",
                         Timestamp = DateTime.UtcNow.AddMinutes(1),
                         IsRead = true
                     },
                      new Message
                      {

                          SenderId = "user3",
                          ReceiverId = "user1",
                          Content = "Hey Bob! How are you?",
                          Timestamp = DateTime.UtcNow.AddMinutes(1),
                          IsRead = true
                      }
                    );
            }
            context.SaveChanges();
        }
    }
}
