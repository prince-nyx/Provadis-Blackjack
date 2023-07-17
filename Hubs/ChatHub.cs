using BlackJack.Gaming;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace BlackJack.Hubs
{
    public class ChatHub : Hub
    {

        private Testing testing;


        public ChatHub()
        {
            Console.WriteLine("ChatHub");
        }
        public async Task SendMessage(string user, string message)
        {
            testing = new Testing(this);
            //testing.sendMessage(user, message);
            await Clients.All.SendAsync("console", "nachricht gesendet");
        }

        public async Task test()
        {
            await Clients.All.SendAsync("Console", "backend event");
        }

        public async Task onConnection()
        {
            string connectionId = Context.ConnectionId;
            testing = new Testing(this);
            Console.WriteLine("Neue Verbindung: " + connectionId);


            await Clients.All.SendAsync("console", "backend event");
            await base.OnConnectedAsync();
        }

        public async Task sendMessagResult(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);

        }
    }
}
