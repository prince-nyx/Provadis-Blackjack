using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlackJack.Pages
{
    public class GameModel : PageModel
    {
        public void OnGet()
        {
            //START ACCESS CHECK
            String userid = Request.Cookies["userid"];
            String result = Program.app.checkAccess(userid);
            if (!result.Equals("/game"))
            {
                Response.Redirect(result);
            }
            //END ACCESS CHECK
        }
    }
}
