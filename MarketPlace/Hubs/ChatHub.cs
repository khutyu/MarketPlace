using Microsoft.AspNetCore.SignalR;


namespace MarketPlace.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(string user,string message)
        {
            Console.WriteLine($"Sending message:{user} says {message} ");
           await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
