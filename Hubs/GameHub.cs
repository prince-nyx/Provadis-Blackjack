using Microsoft.AspNetCore.SignalR;
using System.Formats.Asn1;
using System.Numerics;
using System.Security.Claims;

namespace BlackJack.Hubs
{
    public class GameHub : Hub
    {

        public GameHub()
        {
            Console.WriteLine("Gamehub erstellt");
            Program.gamehub = this;
        }

        public async Task onConnection(String cookie)
        {
       

            Player player = Program.app.playerManager.getPlayer(cookie);
            if(player == null)
                await Clients.All.SendAsync("console", "Kein Login vorhanden");
            else
            {
                //player.hub = this;
                player.connectionId = Context.ConnectionId;
                Game game = Program.app.gameManager.getGame(player.currentGameId);
                if (game != null && game.hostid.Equals(player.id))
                {
                    game.hub = this;
                    game.addPlayer(player);
                }
                await Clients.All.SendAsync("console", "Eingeloggt als " + player.username);

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


        //test
        public async Task testConnection(String message)
        {
            await Clients.All.SendAsync("console", message);

        }

        //all
        public async Task addCardToPlayer(int slotid, String cardname)
        {
            await Clients.All.SendAsync("console", "[EVENT] addCardToPlayer("+slotid +" "+cardname+")");
        }

        //all
        public async Task setCardSum(int slotid, int amount)
        {
            await Clients.All.SendAsync("console", "[EVENT] setCardSum(" + slotid + " " + amount + ")");
        }

        //all
        public async Task assignPlayer(int slotid, String username)
        {
            await Clients.All.SendAsync("console", "[EVENT] assignPlayer(" + slotid + " " + username + ")");
        }

        //all
        public async Task unassignPlayer(int slotid)
        {
            await Clients.All.SendAsync("console", "[EVENT] unassignPlayer(" + slotid + ")");
        }

        //all
        public async Task setBet(int slotid, double amount)
        {
            await Clients.All.SendAsync("console", "[EVENT] setBet(" + amount + ")");
        }

        //all
        public async Task addDealerCard(String cardname, Boolean hidden)
        {
            Console.WriteLine("[EVENT] addDealerCard(" + cardname + ", " + hidden + ")");
            await Clients.All.SendAsync("console", "[EVENT] addDealerCard(" + cardname + ", " + hidden + ")");
        }

        //client
        public async Task endTurn(Player player, int slotid)
        {
            await Clients.Client(player.connectionId).SendAsync("console", "[EVENT] endTurn(" + slotid + ")");       
        }

        //client
        public async Task startTurn(Player player, int time)
        {
            await Clients.Client(player.connectionId).SendAsync("console", "[EVENT] startTurn(" + time + ")");
        }

        //client
        public async Task setbBalance(Player player, int amount)
        {
            await Clients.Client(player.connectionId).SendAsync("console", "[EVENT] setBalance(" + amount + " )");
        }

        //client
        public async Task enableBet(Player player)
        {
            await Clients.Client(player.connectionId).SendAsync("console", "[EVENT] enableBet()");
        }

        //client
        public async Task disableBet(Player player)
        {
            await Clients.Client(player.connectionId).SendAsync("console", "[EVENT] disableBet()");
        }


        //client
        public async Task showResult(Player player, int amount, String result)
        {
            await Clients.Client(player.connectionId).SendAsync("console", "[EVENT] showResult(" + amount + " " + result + ")");
        }

        //client
        public async Task showStartButton(Player player)
        {
            await Clients.Client(player.connectionId).SendAsync("console", "[EVENT] showStartButton()");
        }
    }
}
