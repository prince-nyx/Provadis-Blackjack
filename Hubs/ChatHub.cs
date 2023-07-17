using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace BlackJack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task test()
        {
            await Clients.All.SendAsync("Console", "backend event");
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            Console.WriteLine("Neue Verbindung: " + connectionId);

            await Clients.All.SendAsync("console","backend event");
            await base.OnConnectedAsync();
        }
    }
}
