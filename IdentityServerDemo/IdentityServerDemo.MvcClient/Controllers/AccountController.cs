using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
