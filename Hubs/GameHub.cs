using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlackJack.Hubs
{
    public class GameHub : Hub
    {

        public String connectionId { get; set; }


        public String getConnectionID() { return connectionId; }    
        public GameHub()
        {
            Console.WriteLine("GameHub");
        }


        public async Task update(String cookie)
        {
            connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.Client(connectionId).SendAsync("updated", "Kein Login vorhanden");
            else
            {
                player.connectionId = connectionId;
                foreach(FrontendEvent frontendEvent in player.events)
                {
                    Console.WriteLine("[EVENTS] " + frontendEvent.eventName + " bei " + player.username);
                    await Clients.Client(connectionId).SendAsync("console", frontendEvent.eventName+"("+ string.Join(",", frontendEvent.args) + ")");
                }
                player.events.Clear();
                await Clients.Client(connectionId).SendAsync("updated", "connected als " + player.username);

            }
        }

        public async Task onConnection(String cookie)
        {
            connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if(player == null)
                await Clients.Client(connectionId).SendAsync("console", "Kein Login vorhanden");
            else
            {
                player.hub = this;
                player.connectionId = connectionId;
                //player.hub = this;
                
                player.connectionId = Context.ConnectionId;
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                game.PlayerJoined(player);
                
                
                await Clients.Client(connectionId).SendAsync("console", "Eingeloggt als " + player.username);

            }
        }

        public async Task startGame(String cookie)
        {
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.All.SendAsync("console", "Kein Login vorhanden");
            else
            {
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                if(game == null)        
                    await Clients.All.SendAsync("console", "Game nicht gefunden");
                else
                {
                    game.startGame();
                    await Clients.All.SendAsync("console", "Game gestartet");
                }
            }
        }
    }
}
