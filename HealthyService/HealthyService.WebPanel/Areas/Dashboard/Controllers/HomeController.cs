using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
                model.Products = await dbContext.Products.Where(q => q.Protein > 10).OrderByDescending(q => q.Protein).ToListAsync();

                model.Products2 = await dbContext.Products.Where(q => q.Fat < 5).OrderBy(q => q.Fat).ToListAsync();


                // model.UsersDetails = await dbContext.UsersDetails.OrderBy(q => q.CreateDate).FirstOrDefaultAsync();
                model.User = await dbContext.Users.Where(q => q.IsActive && !q.IsDeleted)
                    .OrderBy(q => q.CreateDate).FirstOrDefaultAsync();

               model.UserDetailsFirst = await dbContext.UsersDetails.Where(q => q.IsActive).OrderBy(q => q.CreateDate).FirstOrDefaultAsync();
               model.UserDetailsLast = await dbContext.UsersDetails.Where(q => q.IsActive).OrderBy(q => q.CreateDate).LastOrDefaultAsync();

                model.UserTest = await dbContext.Users.Where(q => q.IsActive && !q.IsDeleted).Where(q => q.Name == "Ku").FirstOrDefaultAsync();

                //....https://docs.microsoft.com/pl-pl/ef/ef6/querying/related-data
                return View(model);

            }
        }
    }


    public class ProdUsr
    {
        public List<Product> Products { get; set; }
        public List<Product> Products2 { get; set; }
        public User User { get; set; }
        public UserDetails UserDetailsFirst { get; set; }

        public UserDetails UserDetailsLast { get; set; }
        public User UserTest { get; set; }
    }

}