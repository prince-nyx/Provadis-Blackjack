using CookieManager;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BlackJack.Hubs
{
    public class GameHub : Hub
    {
        public async Task StartGame()
        {
            Game game = new Game(this);
            Console.WriteLine("SPIEL GESTARTET");
            game.dealCard();
            game.dealCard();
            //games.Add("test", game);
        }

        public async Task fireEvent(String eventname, params String[] args)
        {
            Console.WriteLine("[EVENTS] "+eventname+" [ "+args.ToString()+" ]");
            
            await Clients.All.SendAsync(eventname, args);
        }


        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            Console.WriteLine(Context.User.Identity.IsAuthenticated);
            Console.WriteLine("user identifier: "+Context.UserIdentifier);
            await base.OnConnectedAsync();
        }



    }
}
