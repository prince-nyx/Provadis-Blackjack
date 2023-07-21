using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using System.Numerics;

namespace BlackJack
{
    public class PlayerManager
    {

        private Dictionary<string, Player> players;

        public PlayerManager()
        {
            players = new Dictionary<string, Player>();
        }

        public void logout(String userid)
        {
            if (players.ContainsKey(userid))
            {
                Player player = players[userid];
                if(player.currentGameId != "")
				{
                   Game game = Program.app.gameManager.getGame(player.currentGameId);
                    if(game != null) {
                        game.PlayerLeaves(player);
                    }
                }
                players.Remove(userid);
            }
        }

        public Boolean login(Player player)
        {
            if(!players.ContainsKey(player.id))
            {
                players.Add(player.id, player);
                Console.WriteLine("[PLAYER] " + player.username + " (ID:" + player.id + ") logged in");
                return true;
            }
            return false;
        }

        public Player getPlayer(String userid)
        {
            return userid == null ? null : players.ContainsKey(userid) ? players[userid] : null;
        }

        public List<Player> getAllPlayers() {
            return players.Values.ToList();
        }
    }
}
