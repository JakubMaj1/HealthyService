using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using HealthyService.WebPanel.Areas.Account.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HealthyService.Core.Logic.Users;
using Microsoft.AspNetCore.Http;
using HealthyService.Core.Database.Tables;

namespace HealthyService.WebPanel.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            //TODO: Sprawdzic w bazie czy uzytkownik istnieje.
            ViewData["ReturnUrl"] = returnUrl;

            var User = await new UserManager().GetUserAsync(model.UserEmailOrLogin);
           
            if(User !=null)
            {
                if(new UserManager().CheckPassword(User.Email, User.Password, model.UserPassword))
                {
                    await SingInAsync(this.HttpContext, User);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("UserDashboard", "Home", new { Area = "Dashboard" });
                    }
                }

                ModelState.AddModelError(String.Empty, "Invalid login attempt.");
                return View(model);
            }
            else
                {
                    return RedirectToAction("RegisterViewModel", "Home", new { Area = "Account" });

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

        [HttpGet]
        public async Task<IActionResult> RegisterUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(Model.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
                {
                    using (var transaction = await dbContext.Database.BeginTransactionAsync())
                    {
                        var md5Passwod = new UserManager().EncodePassword(model.Email, model.Password);

                        await dbContext.Users.AddAsync(new HealthyService.Core.Database.Tables.User
                        {
                            Name = model.Name,
                            SureName = model.SureName,
                            Email = model.Email,
                            Password = md5Passwod,
                            IsActive = true,
                            IsDeleted = false,
                            CreateDate = DateTime.Now,
                            Login = model.Login,

                        });
                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }

            }
            else
            {

            }
            return RedirectToAction("UserDashboard", "Home", new { Area = "Dashboard" });
        }

        public async Task<bool> IsEmailValid(string email)
        {
            using(var dbContext = new Core.Database.HealthyServiceContext())
            {
                var userRef = await dbContext.Users.Where(q => EF.Functions.Like(q.Email, email)).FirstOrDefaultAsync();

                if (userRef == null)
                    return true;
                else
                    return false;
            }
        }

        public async Task SingInAsync(HttpContext httpContext, User user, bool isPersistent = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Login),
                new Claim(ClaimTypes.Name, user.Login),
            };

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new Claim(ClaimTypes.Email, user.Email));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
            };

            await httpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity),
                  authProperties);
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }
    }
}