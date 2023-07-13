using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlackJack
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            Console.WriteLine("LOGIN VIEW");
            return View();
        }

        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            Console.WriteLine("LOGIN");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "JohnDoe") // Beispielhaft einen Anspruch hinzufügen
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}