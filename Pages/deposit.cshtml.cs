using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace BlackJack.Pages
{
	public class DepositModel : PageModel
	{
		public string? depositAmount { get; set; }

		private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
		private SqlCommand? cmd;
		private SqlDataReader? reader;
		private readonly SqlDataAdapter? adapter = new SqlDataAdapter();

        


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
				//Response.Redirect(result);
			}
			//END ACCESS CHECK
		}
		public void OnPost()
		{
			
			this.depositAmount += Request.Form["depositAmount"];
            
            InsertDeposit();
			
        }
        private void InsertDeposit()
        {
            try
            {
                double value;
                value = Convert.ToDouble(this.depositAmount);
                Encrypt encrypt = new();
                string cookie = Request.Cookies["userid"];
                Player player = Program.app.playerManager.getPlayer(cookie);
				if (value <= 100)
				{
					if (player != null)
					{
						player.wallet += value;
						string sql = "UPDATE Benutzer SET GeldAnzahl = @GeldAnzahl WHERE username = @username";

						this.conn.Open();
						this.cmd = new SqlCommand(sql, this.conn);

						this.cmd.Parameters.AddWithValue("@username", player.username);
						this.cmd.Parameters.AddWithValue("@GeldAnzahl", player.wallet);

						this.cmd.ExecuteNonQuery();

						this.cmd.Dispose();
						this.conn.Close();

						Console.WriteLine("INSERT SUCCESS");
					}
				}
				else Console.WriteLine("ZU VIEL");
				
				//POP UP muss noch gemacht werden / Fehlermeldung falls nicht geklappt hat usw!

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}