using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerDemo.MvcClient.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }

        public void SignOut()
        {
            HttpContext.SignOutAsync("Cookies").Wait();
            HttpContext.SignOutAsync("oidc").Wait();
        }
    }
}
