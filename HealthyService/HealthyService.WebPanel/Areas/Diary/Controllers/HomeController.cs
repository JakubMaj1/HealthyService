using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyService.WebPanel.Areas.Diary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyService.Core.Database.Tables;
using System.Dynamic;

namespace HealthyService.WebPanel.Areas.Diary.Controllers
{
    [Area("Diary")]
    public class HomeController : Controller
    {
        public IActionResult FoodDiary()
        {
            return View();
        }


        public async Task<IActionResult> GetProducts(string search)
        {
            dynamic data = new ExpandoObject();
            data.pagination = new ExpandoObject();
            var da = new
            {
                id = 1,
                text = "element1",
                search = search,
            };

            var da1 = new
            {
                id = 2,
                text = "element1",
                search = search,
            };

            var da2 = new
            {
                id = 3,
                text = "element1",
                search = search,
            };

            var da3 = new
            {
                id = 4,
                text = "element1",
                search = search,
            };

            var myData = new[] { da, da1, da2, da3 }.ToList();

            data.results = myData;

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