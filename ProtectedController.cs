using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ProtectedController : Controller
{
    public IActionResult Index()
    {
        return Content("Diese Aktion erfordert eine gültige Bearer-Token-Authentifizierung.");
    }
}