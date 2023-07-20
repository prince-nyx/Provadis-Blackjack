using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

using System.Reflection.PortableExecutable;
using System.Data.SqlClient;


namespace BlackJack.Pages
{
    public class AdminModel : PageModel
    {

        public int betTime { get; set; }
        public int turnTime { get; set; }
        public double potLimit { get; set; }
        public double maxEinzahlung { get; set; }
        public double startguthaben { get; set; }

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
                if (player != null)
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
                if (!isAdmin)
                {
                    if (result.Equals("ingame"))
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
        public void OnPost()
        {
            Console.WriteLine("Methode OnPost() wurde aufgerufen");
            this.betTime = Convert.ToInt32(Request.Form["betTime"]);
            this.turnTime = Convert.ToInt32(Request.Form["turnTime"]);
            this.potLimit = Convert.ToDouble(Request.Form["potLimit"]);
            this.maxEinzahlung = Convert.ToDouble(Request.Form["maxEinzahlung"]);
            this.startguthaben = Convert.ToDouble(Request.Form["startguthaben"]);
            Console.WriteLine("Werte wurden eingetragen");

            if (
                Program.app.settings.betTime != this.betTime ^
                Program.app.settings.turnTime != this.turnTime ^
                Program.app.settings.potLimit != this.potLimit ^
                Program.app.settings.maxEinzahlung != this.maxEinzahlung ^
                Program.app.settings.startguthaben != this.startguthaben
                
                )
                Console.WriteLine("�berpr�fung wurde abgeschlossen");
                Program.app.settings.betTime = this.betTime;
                Program.app.settings.turnTime = this.turnTime;
                Program.app.settings.potLimit = this.potLimit;
                Program.app.settings.maxEinzahlung = this.maxEinzahlung;
                Program.app.settings.startguthaben = this.startguthaben;
            Console.WriteLine("Vor Reload");
                Program.app.settings.reload();
            Console.WriteLine("Reload");


        }
    }
}