using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
<<<<<<< HEAD
=======
using System.Reflection.PortableExecutable;
using System.Data.SqlClient;

>>>>>>> 33ded14887aa561d866c9cf4fc73df7f3d1a7e7b
namespace BlackJack.Pages
{
    public class AdminModel : PageModel
    {
<<<<<<< HEAD
        public int betTime { get; set; }
        public int turnTime { get; set; }
        public double potLimit { get; set; }
        public double maxEinzahlung { get; set; }
        public double startguthaben { get; set; }

        public void OnGet()
        {
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (result.Equals("logout"))
            {
                Response.Cookies.Delete("userid");
            }
            else
            {
                Response.Redirect("/overview");
            }
            //END ACCESS CHECK
        }
        public void OnPost()
        {
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
                Console.WriteLine("Überprüfung wurde abgeschlossen");
                Program.app.settings.betTime = this.betTime;
                Program.app.settings.turnTime = this.turnTime;
                Program.app.settings.potLimit = this.potLimit;
                Program.app.settings.maxEinzahlung = this.maxEinzahlung;
                Program.app.settings.startguthaben = this.startguthaben;
            Console.WriteLine("Vor Reload");
                Program.app.settings.reload();
            Console.WriteLine("Reload");
=======
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
>>>>>>> 33ded14887aa561d866c9cf4fc73df7f3d1a7e7b
        }
    }
}
