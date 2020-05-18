using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using HealthyService.Core.Database.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyService.WebPanel.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class HomeController : Controller
    {

        public IActionResult Index(string redirectUrl = null)
        {
            if (User.Identity.IsAuthenticated)
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
            ProdUsr model = new ProdUsr();

            using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
            {
                var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();

                model.Products = await dbContext.Products.Include(q=>q.User)
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId || EF.Functions.Like(q.User.Login, "admin"))
                    .Where(q => q.Protein > 10).OrderByDescending(q => q.Protein).ToListAsync();

                model.Products2 = await dbContext.Products.Include(q => q.User)
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId || EF.Functions.Like(q.User.Login, "admin"))
                    .Where(q => q.Fat < 5).OrderBy(q => q.Fat).ToListAsync();

               model.UserDetailsFirst = await dbContext.UsersDetails
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId)
                    .Where(q => q.Age != null)
                    .OrderBy(q => q.CreateDate).FirstOrDefaultAsync();

               model.UserDetailsLast = await dbContext.UsersDetails
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId)
                    .Where(q => q.Age != null)
                    .OrderBy(q => q.CreateDate).LastOrDefaultAsync();

                return View(model);

            }
        }
    }


    public class ProdUsr
    {
        public List<Product> Products { get; set; }
        public List<Product> Products2 { get; set; }
        public UserDetails UserDetailsFirst { get; set; }

        public UserDetails UserDetailsLast { get; set; }
    }

}