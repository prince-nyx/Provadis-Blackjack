using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace BlackJack.Pages
{
	public class DepositModel : PageModel
	{
		public string? depositAmount { get; set; }

        public void OnGet()
		{
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (result.Equals("logout"))
            {
                Response.Redirect("/index");
            }

            //END ACCESS CHECK
        }
        public IActionResult OnPost()
		{
			this.depositAmount += Request.Form["depositAmount"];

			double value;
			value = Convert.ToDouble(this.depositAmount);
			Encrypt encrypt = new();
			string cookie = Request.Cookies["userid"];
			Player player = Program.app.playerManager.getPlayer(cookie);
			if (value <= Program.app.settings.maxEinzahlung)
			{
				if (player != null)
				{

					player.AddWallet(value);
					TempData["result"] = "Einzahlung von " + value + " erfolgreich!";
					return RedirectToPage("/overview");
				}
				else
				{
					TempData["error"] = "Spieler nicht gefunden";
					return RedirectToPage("/Error", new { error = "playernotfound" });
				}
			}
			else
			{
				ModelState.AddModelError("depositAmount", "Einzahlung kann nicht über" + Program.app.settings.maxEinzahlung + " liegen.");
			}
			
			return Page();

		}
    }
}