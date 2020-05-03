using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyService.WebPanel.Areas.Diary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthyService.Core.Database.Tables;


namespace HealthyService.WebPanel.Areas.Diary.Controllers
{
    [Area("Diary")]
    public class HomeController : Controller
    {
        public IActionResult FoodDiary()
        {
            return View();
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