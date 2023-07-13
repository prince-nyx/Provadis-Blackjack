using Microsoft.AspNet.SignalR.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BlackJack
{
    public class ProgramController : Controller
    {
        public IActionResult SetCookie()
        {

            string userid = Program.program.loginPlayer("test", "", 0).ToString();

            // Erstelle einen Cookie mit dem Benutzer-Identifikator
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1) // Ablaufdatum des Cookies nach 7 Tagen
            };
            Response.Cookies.Append("UserId", userid, cookieOptions);

            Console.WriteLine("ein cookie mit der id " + userid + " wurde gesetzt");
            return RedirectToAction("Index");
        }
    }
}