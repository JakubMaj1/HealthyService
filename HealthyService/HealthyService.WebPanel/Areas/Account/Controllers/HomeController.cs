using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HealthyService.WebPanel.Areas.Account.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyService.WebPanel.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            //TODO: Sprawdzic w bazie czy uzytkownik istnieje.

            var claims = new List<Claim>
            {
                new Claim("user", model.UserEmail),
                new Claim("role", "Member")
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme,"user","role")));

            if(Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("UserDashboard", "Home", new { Area = "Dashboard" });
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home", new { Area = "Dashboard" });
        }
        //[Authorize(AuthenticationSchemes = "Facebook")]
        //public async Task<IActionResult> LoginFacebook(string redirectUrl = null)
        //{
        //    //TODO: Sprawwdzic w bazie czy email istnieje jak nie to zarejestrowac automatyucznie i dodac usera

        //    if (Url.IsLocalUrl(redirectUrl))
        //    {
        //        return Redirect(redirectUrl);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home", new { Area = "Dashboard" });
        //    }
        //}

        public async Task<IActionResult> AccessDenied(long id)
        {
            return View();
        }
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserDashboard", "Home", new { Area = "Dashboard" });
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }
        public async Task<IActionResult> Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserDashboard", "Home", new { Area = "Dashboard" });
            }
            return View();
        }

    }
}