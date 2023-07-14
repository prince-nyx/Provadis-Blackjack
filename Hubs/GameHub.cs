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
                player.hub = this;
                player.connectionId = Context.ConnectionId;
                Console.WriteLine($"{player.connectionId} Connection ID");
                await Clients.All.SendAsync("console", "Eingeloggt als " + player.username);

            }
        }

        public async Task addCardToPlayer(int slotid, String cardname)
        {
            await Clients.All.SendAsync("console", "[EVENT] addCardToPlayer("+slotid +" "+cardname+")");
        }

        public async Task setCardSum(int slotid, int amount)
        {
            await Clients.All.SendAsync("console", "[EVENT] setCardSum(" + slotid + " " + amount + ")");
        }
        public async Task assignPlayer(int slotid, String username)
        {
            await Clients.All.SendAsync("console", "[EVENT] assignPlayer(" + slotid + " " + username + ")");
        }
        public async Task unassignPlayer(int slotid)
        {
            await Clients.All.SendAsync("console", "[EVENT] unassignPlayer(" + slotid + ")");
        }
        public async Task endTurn(int slotid)
        {
            await Clients.All.SendAsync("console", "[EVENT] endTurn(" + slotid + ")");       
        }
        public async Task startTurn(int slotid, int time)
        {
            await Clients.All.SendAsync("console", "[EVENT] startTurn(" + slotid + " " + time + ")");
        }
        public async Task setbBalance(Player player, int amount)
        {
            await Clients.All.SendAsync("console", "[EVENT] setbBalance(" + amount + " )");
        }
        public async Task enableBet(Player player)
        {
            await Clients.All.SendAsync("console", "[EVENT] enableBet()");
        }
        public async Task disableBet(Player player)
        {
            await Clients.All.SendAsync("console", "[EVENT] disableBet()");
        }
        public async Task setBet(int slotid, double amount)
        {
            await Clients.All.SendAsync("console", "[EVENT] setBet(" + amount + ")");
        }
        public async Task showResult(Player player, int amount, String result)
        {
            await Clients.All.SendAsync("console", "[EVENT] showResult(" + amount + " " + result + ")");
        }
        public async Task addDealerCard(String cardname)
        {
            await Clients.All.SendAsync("console", "[EVENT] addDealerCard(" + cardname + ")");
        }
        public async Task showStartButton(Player player)
        {
            await Clients.All.SendAsync("console", "[EVENT] showStartButton()");
        }
    }
}
