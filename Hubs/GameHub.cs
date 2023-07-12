using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;

namespace BlackJack.Hubs
{
    public class GameHub : Hub
    {
        Game game;
        public async Task StartGame()
        {
            Console.WriteLine("GAME STARTET");
            game = new Game(this);
            Player hans = new Player("Hans");
            game.addPlayer(hans);

            Console.WriteLine(hans.ToString());
            Console.WriteLine("SPIEL GESTARTET");
            game.dealCard();
            game.dealCard();
            Console.WriteLine(hans.ToString());
        }

        public async Task fireEvent(String eventname, params String[] args)
        {
            Console.WriteLine("[EVENTS] "+eventname+" [ "+args.ToString()+" ]");
            await Clients.All.SendAsync(eventname, args);
        }




    }
}
