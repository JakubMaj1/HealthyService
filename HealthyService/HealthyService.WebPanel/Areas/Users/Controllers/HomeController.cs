using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using HealthyService.Core.Database.Tables;
using HealthyService.WebPanel.Areas.Users.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace HealthyService.WebPanel.Areas.Users.Controllers
{
    [Area("Users")]
    public class HomeController : Controller
    {

        public async Task<IActionResult> MacroKcalAdd()
        {
            //var userEmail = this.HttpContext.User.Identity.Name; 1

            MacroKcalAddModel model = new MacroKcalAddModel();

            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                //var myDetails = await dbContext.UsersDetails.Include(q=>q.User)2
                //    .Where(q=>q.IsActive && !q.IsDeleted)
                //    .Where(q=>q.User.IsActive && !q.User.IsDeleted)
                //    .Where(q => EF.Functions.Like(q.User.Email, userEmail))
                //    .OrderByDescending(q => q.CreateDate).FirstOrDefaultAsync();

                var Macro = await dbContext.UsersDetails.Where(q => q.IsActive && !q.IsDeleted).OrderByDescending(q => q.CreateDate).FirstOrDefaultAsync();
                if(Macro != null)
                {
                    model.UserCarboLevel = Macro.UserCarboLevel;
                    model.UserFatLevel = Macro.UserFatLevel;
                    model.UserProteinLevel = Macro.UserProteinLevel;
                }

                return View(model);
            } 
        }
        [HttpPost]
        public async Task <IActionResult> MacroKcalAdd (MacroKcalAddModel model)
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    var LastMacro = await dbContext.UsersDetails.Where(q => q.IsActive && !q.IsDeleted).OrderByDescending(q => q.CreateDate).FirstOrDefaultAsync();

                    UserDetails userDetails = UserDetailsDto.Copy(LastMacro);

                    userDetails.UserCarboLevel = model.UserCarboLevel;
                    userDetails.UserProteinLevel = model.UserProteinLevel;
                    userDetails.UserFatLevel = model.UserFatLevel;
                    userDetails.UserDemendLevel = model.UserDemendLevel;
                    userDetails.CreateDate = DateTime.Now;

                    await dbContext.UsersDetails.AddAsync(userDetails);

                    await dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
            }
        

            return RedirectToAction("Index", "Home", new { Area = "Dashboard" });

        }

        [HttpGet]
        public async Task <IActionResult> MeasurementAdd()
        {
            Dictionary<string, string> Translator2 = new Dictionary<string, string>();
            Translator2.Add(Core.Database.Types.ActivityLevelType.Small.ToString(), "Mała aktywność");
            Translator2.Add(Core.Database.Types.ActivityLevelType.Medium.ToString(), "Średnia aktywność");
            Translator2.Add(Core.Database.Types.ActivityLevelType.Large.ToString(), "Duża aktywność");
            Translator2.Add(Core.Database.Types.ActivityLevelType.ExtraLarge.ToString(), "Bardzo duża aktywność");

            Model.MeasurementAddModel model = new Model.MeasurementAddModel();

            var types = new List<SelectListItem>();

            types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Small.ToString()], Value = Core.Database.Types.ActivityLevelType.Small.ToString() });
            types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Medium.ToString()], Value = Core.Database.Types.ActivityLevelType.Medium.ToString() });
            types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Large.ToString()], Value = Core.Database.Types.ActivityLevelType.Large.ToString() });
            types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.ExtraLarge.ToString()], Value = Core.Database.Types.ActivityLevelType.ExtraLarge.ToString() });

            model.ActivityLevels = new SelectList(types, "Value", "Text");

            Dictionary<string, string> Translator3 = new Dictionary<string, string>();

            Translator3.Add(Core.Database.Types.GenderType.Woman.ToString(), "Kobieta");
            Translator3.Add(Core.Database.Types.GenderType.Man.ToString(), "Mężczyzna");

            var types1 = new List<SelectListItem>();
            

            types1.Add(new SelectListItem() { Text = Translator3[Core.Database.Types.GenderType.Woman.ToString()], Value = Core.Database.Types.GenderType.Woman.ToString() });
            types1.Add(new SelectListItem() { Text = Translator3[Core.Database.Types.GenderType.Man.ToString()], Value = Core.Database.Types.GenderType.Man.ToString() });

            model.Genders = new SelectList(types1, "Value", "Text");


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> MeasurementAdd(MeasurementAddModel model)
        {
            if (ModelState.IsValid)
            {
                using (var dbContext = new Core.Database.HealthyServiceContext())
                {
                    using (var transaction = await dbContext.Database.BeginTransactionAsync())
                    {

                        var LastMacro = await dbContext.UsersDetails.Where(q => q.IsActive && !q.IsDeleted).OrderByDescending(q => q.CreateDate).FirstOrDefaultAsync();

                        UserDetails userDetails = UserDetailsDto.Copy(LastMacro);



                        Core.Database.Types.ActivityLevelType activityLevel = Core.Database.Types.ActivityLevelType.Small;
                        if(Enum.TryParse<Core.Database.Types.ActivityLevelType>(model.ActivityLevel,out activityLevel))
                        {

                        }
                        Core.Database.Types.GenderType gender = Core.Database.Types.GenderType.Man;
                        if (Enum.TryParse<Core.Database.Types.GenderType>(model.Gender,out gender))
                        {

                        }



                            userDetails.Age = model.Age;
                            userDetails.Height = model.Height;
                            userDetails.Weight = model.Weight;
                            userDetails.WaistCircumference = model.WaistCircumference;
                            userDetails.ActivityLevel = activityLevel;
                            userDetails.ArmCircumference = model.ArmCircumference;
                            userDetails.CalfCircumference = model.CalfCircumference;
                            userDetails.ChestCircumference = model.ChestCircumference;
                            userDetails.HipCircumference = model.HipCircumference;
                            userDetails.Gender = gender;
                            userDetails.CreateDate = DateTime.Now;

                        await dbContext.UsersDetails.AddAsync(userDetails);

                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    

                }
            }

              
            else
            {

            }
            return RedirectToAction("Index", "Home", new { Area = "Dashboard" });
        }
        public IActionResult MeasurementEdit()
        {
            return View();
        }


    }
}