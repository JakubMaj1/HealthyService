using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyService.Core.Database.Tables;
using HealthyService.WebPanel.Areas.Products.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyService.WebPanel.Areas.Products.Controllers
{
    
    [Area("Products")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            List<Product> products = new List<Product>();
                using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
                {
                   products = await dbContext.Products.ToListAsync();
                }

                return View(products);
        }

        public IActionResult AdminAddProduct()
        {

            return View();
        }


        [HttpPost]
        public async Task <IActionResult> AdminAddProduct(Model.AdminAddProductModel model)
        {
            using(var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
            {
                using(var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    await dbContext.Products.AddAsync(new HealthyService.Core.Database.Tables.Product
                    {
                        Name = model.Name,
                        Protein =model.Protein,
                        Carbo= model.Carbo,
                        Fat = model.Fat
                    });
                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }

            return RedirectToAction("Index", "Home", new { Area = "Products" });
        }
    }
}

