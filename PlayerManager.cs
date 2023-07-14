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
            return players.ContainsKey(userid) ? players[userid] : null;
        }
    }
}
