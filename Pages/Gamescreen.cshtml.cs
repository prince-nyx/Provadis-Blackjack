using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlackJack.Pages
{
    public class GameModel : PageModel
    {

        private Game game;
        public void OnGet()
        {
            game = new Game();
            Player hans = new Player("Hans");
            game.addPlayer(hans);

            Console.WriteLine(hans.ToString());
            Console.WriteLine("SPIEL GESTARTET");
            game.dealCard();
            game.dealCard();
            Console.WriteLine(hans.ToString());
        }
    }
}
