using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BlackJack.Pages
{
    public class Index1Model : PageModel
    {
        private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
        private SqlCommand? cmd;
        private SqlDataReader? reader;
        private readonly SqlDataAdapter? adapter = new SqlDataAdapter();

        public void OnGet()
        {
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (!result.Equals("/overview"))
            {
                //Response.Redirect(result);
            }
            //END ACCESS CHECK
        }
        public void OnPost()
        {

            String userid = Request.Cookies["userid"];
            String gamecode = Request.Form["code"];
            GameManger manager = Program.app.gameManager;

            if (gamecode == null || !manager.exist(gamecode.ToUpper()))
                gamecode = manager.createGame();
            Player player = Program.app.playerManager.getPlayer(userid);
            if(player != null)
            {
                if(manager.join(player, gamecode))
                    Response.Redirect("Game");
                else
                    Response.Redirect("Error");
            } else
            {
                Response.Redirect("Error");
            }

        }

    }
}

    
