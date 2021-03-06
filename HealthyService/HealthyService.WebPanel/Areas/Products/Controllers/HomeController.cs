﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyService.Core.Database.Tables;
using HealthyService.WebPanel.Areas.Products.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace HealthyService.WebPanel.Areas.Products.Controllers
{
    
    [Area("Products")]
    public class HomeController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> EditProduct(Model.AdminAddEditProductModel model)
        {
            using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
            {

                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {

                    var product = await dbContext.Products.FindAsync(model.ProductId);

                    if (product == null)
                        return View(model);
                    else
                    {
                        product.Carbo = (float)model.Carbo;
                        product.Fat = (float)model.Fat;
                        product.Protein = (float)model.Protein;
                        product.Name = model.Name;
                        product.ProductMeasure = model.ProductMeasure;
                        product.IsActive = model.IsActive;
                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }
            }

            return RedirectToAction("Index", "Home", new { Area = "Products" });

        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(long productId)
        {

            var model = new AdminAddEditProductModel();
            Dictionary<Core.Database.Types.ProductMeasureType, string> Translator = new Dictionary<Core.Database.Types.ProductMeasureType, string>();

            Translator.Add(Core.Database.Types.ProductMeasureType.Piece, "Sztuka");
            Translator.Add(Core.Database.Types.ProductMeasureType.Gram, "Gramy");

            var types1 = new List<SelectListItem>();


            types1.Add(new SelectListItem() { Text = Translator[Core.Database.Types.ProductMeasureType.Piece], Value = Core.Database.Types.ProductMeasureType.Piece.ToString() });
            types1.Add(new SelectListItem() { Text = Translator[Core.Database.Types.ProductMeasureType.Gram], Value = Core.Database.Types.ProductMeasureType.Gram.ToString() });

            model.ProductMeasures = new SelectList(types1, "Value", "Text");

            using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
            {
                
                var product = await dbContext.Products.FindAsync(productId);

                if(product == null)
                    return View(model);
                else
                {
                    model.Carbo = (decimal)product.Carbo;
                    model.Fat = (decimal)product.Fat;
                    model.Name = product.Name;
                    model.ProductMeasure = product.ProductMeasure;
                    model.Protein = (decimal)product.Protein;
                    model.ProductId = product.Id;
                    model.ProductId = productId;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Index()
        {

            List<Product> products = new List<Product>();
                using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
                {
                   products = await dbContext.Products.ToListAsync();
                }

                return View(products);
        }
        

      
        [HttpGet]
        public async Task <IActionResult> AdminAddProduct()
        {

            Model.AdminAddEditProductModel model = new Model.AdminAddEditProductModel();

            Dictionary<Core.Database.Types.ProductMeasureType, string> Translator = new Dictionary<Core.Database.Types.ProductMeasureType, string>();

            Translator.Add(Core.Database.Types.ProductMeasureType.Piece, "Sztuka");
            Translator.Add(Core.Database.Types.ProductMeasureType.Gram, "Gramy");

            var types1 = new List<SelectListItem>();


            types1.Add(new SelectListItem() { Text = Translator[Core.Database.Types.ProductMeasureType.Piece], Value = Core.Database.Types.ProductMeasureType.Piece.ToString() });
            types1.Add(new SelectListItem() { Text = Translator[Core.Database.Types.ProductMeasureType.Gram], Value = Core.Database.Types.ProductMeasureType.Gram.ToString() });

            model.ProductMeasures = new SelectList(types1, "Value", "Text");


            return View(model);
        }

        [HttpPost]
        public async Task <IActionResult> AdminAddProduct(Model.AdminAddEditProductModel model)
        {
            if(ModelState.IsValid)
            {
                using (var dbContext = new HealthyService.Core.Database.HealthyServiceContext())
                {
                    using (var transaction = await dbContext.Database.BeginTransactionAsync())
                    {
                        var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                        var UserId = await dbContext.Users
                            .Where(q => EF.Functions.Like(q.Login, UserLogin))
                            .Where(q => q.IsActive && !q.IsDeleted)
                            .Select(q => q.Id).FirstOrDefaultAsync();


                        await dbContext.Products.AddAsync(new HealthyService.Core.Database.Tables.Product
                        {
                            Name = model.Name,
                            Protein = (float)model.Protein,
                            Carbo = (float)model.Carbo,
                            Fat = (float)model.Fat,
                            ProductMeasure = model.ProductMeasure,
                            UserId = UserId
                        });
                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                }
            }

            return RedirectToAction("Index", "Home", new { Area = "Products" });
        }
    }
}

