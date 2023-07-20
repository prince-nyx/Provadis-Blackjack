using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;

namespace BlackJack.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

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
    }
}