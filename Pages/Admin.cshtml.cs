using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace BlackJack.Pages
{
    public class AdminModel : PageModel
    {
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
        }
    }
}
