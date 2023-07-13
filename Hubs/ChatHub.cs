﻿using Microsoft.AspNetCore.SignalR;

namespace BlackJack.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            Console.WriteLine("Neue Verbindung: " + connectionId);
            await base.OnConnectedAsync();
        }
    }
}
