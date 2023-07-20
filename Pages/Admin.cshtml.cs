using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.PortableExecutable;
using System.Data.SqlClient;

namespace BlackJack.Pages
{
    public class AdminModel : PageModel
    {
        private SqlCommand? cmd;
        private SqlDataReader? reader;
        private readonly SqlDataAdapter? adapter = new SqlDataAdapter();
        private readonly SqlConnection conn = new("Server=provadis-it-ausbildung.de;Database=BlackJack02;UID=BlackJackUser02;PWD=Pr@vadis_188_Pta;");
        public void OnGet()
        {
            Boolean isAdmin = false;
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (result.Equals("ingame") || result.Equals("loggedin"))
            {
                Player player = Program.app.playerManager.getPlayer(userid);
                if(player != null)
                {
                    string sql = "SELECT DISTINCT isAdmin FROM Benutzer WHERE Username = @Username";

                    this.conn.Open();
                    this.cmd = new SqlCommand(sql, this.conn);
                    this.cmd.Parameters.AddWithValue("@Username", player.username);
                    this.reader = this.cmd.ExecuteReader();
                    while (this.reader.Read())
                    {
                        isAdmin = reader.GetBoolean(0);

                    }
                    this.reader.Close();
                    this.cmd.Dispose();
                    this.conn.Close();
                }
                if(!isAdmin)
                {
                    if(result.Equals("ingame"))
                        Response.Redirect("/game");
                    else
                        Response.Redirect("/overview");
                }
            }
            else if (result.Equals("logout"))
            {
                Response.Redirect("/index");
            }
        }
    }
}
