using Microsoft.AspNetCore.SignalR;

namespace BlackJack.Hubs
{
    public class HubDatenbank : Hub
    {
        public async Task SendMessage(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
