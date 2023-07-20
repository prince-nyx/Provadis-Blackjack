using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BlackJack.Pages
{
    public class Index1Model : PageModel
    {
        public string? code { get; set; }
        public void OnGet()
        {
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (result.Equals("ingame"))
            {
                Response.Redirect("/game");
            } else if(result.Equals("logout"))
            {
                Response.Redirect("/index?info=playernotfound");
            }



            //END ACCESS CHECK
        }
        public IActionResult OnPost()
        {

            String userid = Request.Cookies["userid"];
            String gamecode = Request.Form["code"];
            GameManger gameManager = Program.app.gameManager;
            Player player = Program.app.playerManager.getPlayer(userid);
            if(player != null)
            {
                //Spiel erstellen wenn Code nicht vorhanden
                if(gamecode == "")
                {
                    gamecode = gameManager.createGame(player.id);
                }


                // Code überprüfen
                if(gameManager.exist(gamecode.ToUpper()))
                {
                    if(gameManager.join(player, gamecode.ToUpper()))
                    {
                        return RedirectToPage("/Game");
                    } else
                    {
                        ModelState.AddModelError("code", "Spielbeitritt nicht möglich, da Spiel läuft");
                        return Page();

                    }
                } else
                {
                    ModelState.AddModelError("code", "Spiel nicht gefunden");
                    return Page();
                }
            } else
            {
                return RedirectToPage("/index");
            }

        }

    }
}

    
