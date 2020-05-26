using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyService.WebPanel.Areas.Diary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyService.Core.Database.Tables;
using System.Dynamic;
using System.Security.Claims;

namespace HealthyService.WebPanel.Areas.Diary.Controllers
{
    [Area("Diary")]
    public class HomeController : Controller
    {
        public IActionResult FoodDiary()
        {
            return View();
        }

        public async Task<IActionResult> GetMealList()
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();

                //Pobierz posilki
                var mealProductList = await dbContext.Meals.AsNoTracking()
                    .Include(q => q.Products)
                    .ThenInclude(q => q.Product)
                    .Where(q => q.UserId == UserId)
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.MealDateTime.Date == DateTime.Now.Date).ToListAsync();

                return PartialView(mealProductList);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAmountToProdcut(long mealId, long productId, float newValue)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    //Pobierz posilek
                    var mealProduct = await dbContext.ProductMeals
                        .Where(q => q.MealId == mealId)
                        .Where(q => q.ProductId == productId).FirstOrDefaultAsync();

                    mealProduct.Amount = newValue;

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json("ok");
                }
            }
        }

        public async Task<IActionResult> AddProductToMeal(long mealId, long productId)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                    var UserId = await dbContext.Users
                        .Where(q => EF.Functions.Like(q.Login, UserLogin))
                        .Where(q => q.IsActive && !q.IsDeleted)
                        .Select(q => q.Id).FirstOrDefaultAsync();

                    //Pobierz posilek
                    var meal = await dbContext.Meals.FindAsync(mealId);

                    //Pobierz produkt
                    var product = await dbContext.Products.FindAsync(productId);

                    var productMeal = new ProductMeal();

                    productMeal.ProductId = productId;
                    productMeal.MealId= mealId;

                    if (product.ProductMeasure == Core.Database.Types.ProductMeasureType.Gram)
                        productMeal.Amount = 100;
                    else
                        productMeal.Amount = 1;

                    await dbContext.ProductMeals.AddAsync(productMeal);

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json("ok");
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMealName(long mealId, string name)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    //Pobierz posilek
                    var meal = await dbContext.Meals.FindAsync(mealId);

                    meal.Name = name;
                  
                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json("ok");
                }
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProductFromMeal(long mealId, long productId)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    //Pobierz posilek
                    var mealproduct = await dbContext.ProductMeals
                        .Where(q => q.ProductId == productId)
                        .Where(q => q.MealId == mealId).FirstOrDefaultAsync();

                    dbContext.ProductMeals.Remove(mealproduct);

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json("ok");
                }
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteMealFromDiary(long mealId)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                   
                    var meal = await dbContext.Meals.FindAsync(mealId);

                    meal.IsDeleted = true;

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json("ok");
                }
            }
        }

        public async Task<IActionResult> GetCreateMeal(long mealId)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();


                //Pobierz posilek po id
                var meal = await dbContext.Meals.AsNoTracking()
                    .Include(q => q.Products).ThenInclude(q => q.Product)
                    .Where(q => q.Id == mealId).FirstOrDefaultAsync();
                
                return PartialView(meal);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeal()
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                    var UserId = await dbContext.Users
                        .Where(q => EF.Functions.Like(q.Login, UserLogin))
                        .Where(q => q.IsActive && !q.IsDeleted)
                        .Select(q => q.Id).FirstOrDefaultAsync();


                    //Pobierz ilosc posilkow na dzis
                    var mealCount = await dbContext.Meals.AsNoTracking()
                        .Where(q => q.UserId == UserId)
                        .Where(q => q.MealDateTime.Date == DateTime.Now.Date).CountAsync();


                    var meal = new HealthyService.Core.Database.Tables.Meal();

                    meal.MealDateTime = DateTime.Now;
                    meal.UserId = UserId;
                    meal.Name = $"Posiłek {mealCount + 1}";

                    await dbContext.Meals.AddAsync(meal);

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Json(meal.Id);
                }
            }
        }

        public async Task<IActionResult> GetProducts(string search)
        {
            dynamic data = new ExpandoObject();
            data.pagination = new ExpandoObject();

            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();


                //Pobierz posilek po id
                var query = dbContext.Products.AsNoTracking()
                    .Where(q => q.UserId == UserId || EF.Functions.Like(q.Name, "admin"));

                if(!string.IsNullOrEmpty(search))
                {
                    query = query.Where(q => EF.Functions.Like(q.Name, $"%{search}%"));
                }

                var products = await query.ToListAsync();

                data.results = products.Select(q => new { id = q.Id, text = q.Name }).ToArray();
            }

            return Json(data);
        }

        [HttpPost]
        public async Task <IActionResult>FoodDiary(FoodDiaryModel model)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())

            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    await dbContext.Products.AddAsync(new HealthyService.Core.Database.Tables.Product
                    {
                      Name = model.Name,
                      Carbo = model.Carbo,
                      Protein = model.Protein,
                      Fat = model.Fat,
                      

                    });
                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    
                }

            }



            return View();
    }


}
}