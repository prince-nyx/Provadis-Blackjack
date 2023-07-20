using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;

namespace BlackJack.Pages
{
    public class LoginModel : PageModel
    {
		public string? Username { get; set; }
		public string? Password { get; set; }
		private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
		private SqlCommand? cmd;
		private SqlDataReader? reader;

        public void OnGet()
        {
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (result.Equals("/index"))
            {
                Response.Cookies.Delete("userid");
            }
            else
            {
                Response.Redirect(result);
            }
            //END ACCESS CHECK
        }

        public IActionResult OnPost()
        {
			this.Username = Request.Form["username"];
			this.Password = Request.Form["password"];

			try
            {
                Player player = selectPlayer();
                if (player != null)
                {
                    if (Program.app.playerManager.login(player))
                    {
                        Console.WriteLine("Valid");
                        Response.Cookies.Append("userid", player.id);
                        return RedirectToPage("/overview");
                    }
                }
                else
                {

                    ModelState.AddModelError("Password", "Benutzername und Passwort stimmen nicht überein.");
				}
            }
            catch (Exception ex) { 
                
                Console.WriteLine(ex.Message);
			}
			return Page();
		}

        private Player selectPlayer()
        {
            Encrypt encrypt = new();

            bool isValid = false;
            string sql = "SELECT DISTINCT Username, GeldAnzahl FROM Benutzer WHERE Username ='" + this.Username + "' AND Passwort='" + encrypt.GetHashedPassword(this.Password) + "'";
            
            this.conn.Open();
            this.cmd = new SqlCommand(sql, this.conn);
            this.reader = this.cmd.ExecuteReader();
            Player player = null;
            while (this.reader.Read()) {

                string username = reader.GetString(0);
                double wallet = reader.GetDouble(1);
                Console.WriteLine(username + "" + wallet);
                player = new Player(username, wallet);
            }

            this.reader.Close();
            this.cmd.Dispose();
            this.conn.Close();

            return player;
        }
    }
}