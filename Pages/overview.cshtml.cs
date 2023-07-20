using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BlackJack.Pages
{
    public class Index1Model : PageModel
    {
        public string? code1 { get; set; }
        public string? code2 { get; set; }
        public string? code3 { get; set; }
        public string? code4 { get; set; }

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
            String code1 = Request.Form["code1"];
            String code2 = Request.Form["code2"];
            String code3 = Request.Form["code3"];
            String code4 = Request.Form["code4"];
            String gamecode = code1+code2 + code3 + code4;  

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
                        ModelState.AddModelError("code1", "Spielbeitritt nicht möglich, da Spiel läuft");
                        return Page();

                    }
                } else
                {
                    ModelState.AddModelError("code1", "Spiel nicht gefunden");
                    return Page();
                }
            } else
            {
                return RedirectToPage("/index");
            }

        }

    }
}

    
