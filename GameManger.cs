namespace BlackJack
{
    public class GameManger
    {

        private Dictionary<string, Game> games;

        public GameManger()
        {
            games = new Dictionary<string, Game>();
        }


        public Boolean join(Player player, String gameid)
        {
            if (games.ContainsKey(gameid))
            {
                Game game = games[gameid];
                if(!game.containsPlayer(player.id))
                {
                    game.addPlayer(player);
                    player.currentGameId = gameid;
                    Console.WriteLine("[GAMEMANAGER] " + player.ToString() + " joined " + game.ToString());
                }
                else
                    Console.WriteLine("[GAMEMANAGER] " + player.ToString() + " is already in " + game.ToString());
                return true;
            } else
                return false;
        }

        public Boolean exist(String gameid)
        {
            return games.ContainsKey(gameid);
        }

        public String createGame()
        {
            Game game = new Game();
            games.Add(game.id, game);
            return game.id;
        }
    }
}
