using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlackJack.Hubs
{
    public class GameHub : Hub
    {


        public async Task update(String cookie)
        {
            String connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.Client(connectionId).SendAsync("console", "Kein Login vorhanden");
            else
            {
                player.connectionId = connectionId;
                foreach(FrontendEvent frontendEvent in player.events)
                {
                    Console.WriteLine("[EVENTS] " + frontendEvent.eventName + " bei " + player.username);
                    await Clients.Client(connectionId).SendAsync("console", frontendEvent.eventName+"("+ string.Join(",", frontendEvent.args) + ")");
                }
                player.events.Clear();
                await Clients.Client(connectionId).SendAsync("console", "connected als " + player.username);

            }
        }

        public async Task onConnection(String cookie)
        {
            String connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if(player == null)
                await Clients.Client(connectionId).SendAsync("console", "Kein Login vorhanden");
            else
            {
                player.hub = this;
                player.connectionId = connectionId;
           
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                game.PlayerJoined(player);
                
                
                await Clients.Client(connectionId).SendAsync("console", "Eingeloggt als " + player.username);

            }
        }

        public async Task startGame(String cookie)
        {
            String connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.Client(connectionId).SendAsync("console", "Kein Login vorhanden");
            else
            {
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                if(game == null)        
                    await Clients.Client(connectionId).SendAsync("console", "Game nicht gefunden");
                else
                {
                    if(game.hostid!= null && game.hostid.Equals(player.id))
                    {
                        game.startGame();
                        await Clients.Client(connectionId).SendAsync("console", "Game gestartet");
                    } else
                    {
                        await Clients.Client(connectionId).SendAsync("console", "Du bist nicht der Host");
                    }
                }
            }
        }

        public async Task stand(String cookie)
        {
            String connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.Client(connectionId).SendAsync("console", "Dieser Account ist kein Spieler.");
            else
            {
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                if (game == null)
                    await Clients.Client(connectionId).SendAsync("console", "Dieser Spieler ist nicht in diesem Game.");
                else
                {
                    if (game.isPlayersTurn(cookie))
                    {
                        await Clients.Client(connectionId).SendAsync("console", "Spieler möchte keine weitere Karte");
                    }
                    else
                        await Clients.Client(connectionId).SendAsync("console", "Spieler ist nicht dran");
                }
            }
        }
        public async Task hit(String cookie)
        {
            String connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.Client(connectionId).SendAsync("console", "Dieser Account ist kein Spieler.");
            else
            {
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                if (game == null)
                    await Clients.Client(connectionId).SendAsync("console", "Dieser Spieler ist nicht in diesem Game.");
                else
                {
                    if (game.isPlayersTurn(cookie))
                    {
                        game.hit();
                        await Clients.Client(connectionId).SendAsync("console", "Spieler zieht Karte");
                    }
                    else
                        await Clients.Client(connectionId).SendAsync("console", "Spieler ist nicht dran");
                }
            }
        }

        public async Task setBet(String cookie, int amount)
        {
            String connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.Client(connectionId).SendAsync("console", "Dieser Account ist kein Spieler.");
            else
            {
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                if (game == null)
                    await Clients.Client(connectionId).SendAsync("console", "Dieser Spieler ist nicht in diesem Game.");
                else if (game.phase == GamePhase.BETTING)
                {
                    player.AddBet(amount);
                    await Clients.Client(connectionId).SendAsync("console", "Spieler erhöht seinen Einsatz um " + amount);
                }
                else
                    await Clients.Client(connectionId).SendAsync("console", "Derzeit kann man nicht setzen.");
                
            }
        }

        public async Task submitBet(String cookie, int amount)
        {
            String connectionId = Context.ConnectionId;
            Player player = Program.app.playerManager.getPlayer(cookie);
            if (player == null)
                await Clients.Client(connectionId).SendAsync("console", "Dieser Account ist kein Spieler.");
            else
            {
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                if (game == null)
                    await Clients.Client(connectionId).SendAsync("console", "Dieser Spieler ist nicht in diesem Game.");
                else if (game.phase == GamePhase.BETTING)
                {
                    game.submitBet(player.id);
                    await Clients.Client(connectionId).SendAsync("console", "Spieler ist mit dem Einsetzen fertig");
                }
                else
                    await Clients.Client(connectionId).SendAsync("console", "Derzeit kann man nicht setzen.");

            }
        }
        
    }
}
