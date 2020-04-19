using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthyService.WebPanel.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : Controller
    {

        public IActionResult Index(string redirectUrl = null)
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserDashboard", "Home", new { Area = "Dashboard" });
            }
            else
            {
                Account.Model.LoginViewModel model = new Account.Model.LoginViewModel();

                return View(model);
            }
           
        }

        [Authorize]
        public async Task<IActionResult> UserDashboard()
        {
            return View();
        }
    }
}