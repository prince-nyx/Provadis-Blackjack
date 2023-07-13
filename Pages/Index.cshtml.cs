using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

            Response.Cookies.Append("test", "jo");
            Console.WriteLine(this);
            Console.WriteLine(this.HttpContext);
            Console.WriteLine(this.HttpContext.Items);
            foreach(var item in this.HttpContext.Items)
            {
                Console.WriteLine(item.Value.ToString());
            }

        }
    }
}