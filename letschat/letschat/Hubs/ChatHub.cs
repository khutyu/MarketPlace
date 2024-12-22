using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
namespace letschat.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Broadcast message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPrivateMessage(string fromUser, string toUser, string message)
        {
            // Send message only to the specified user
            await Clients.User(toUser).SendAsync("ReceivePrivateMessage", fromUser, message, DateTime.Now);
        }
        public override async Task OnConnectedAsync()
        {
            //tracking online users)
            await base.OnConnectedAsync();
        }
    }
}
