using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
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
                    var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                    var UserId = await dbContext.Users
                        .Where(q => EF.Functions.Like(q.Login, UserLogin))
                        .Where(q => q.IsActive && !q.IsDeleted)
                        .Select(q => q.Id).FirstOrDefaultAsync();

                    UserDetails userDetails = new UserDetails();

                    var LastMacro = await dbContext.UsersDetails.Where(q => q.IsActive && !q.IsDeleted).OrderByDescending(q => q.CreateDate).FirstOrDefaultAsync();
    
                    if(LastMacro != null)
                        userDetails = UserDetailsDto.Copy(LastMacro);

                    userDetails.UserCarboLevel = model.UserCarboLevel;
                    userDetails.UserProteinLevel = model.UserProteinLevel;
                    userDetails.UserFatLevel = model.UserFatLevel;
                    userDetails.UserDemendLevel = model.UserDemendLevel;
                    userDetails.CreateDate = DateTime.Now;
                    userDetails.UserId = UserId;

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
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();

                UserDetails userDetails = new UserDetails();

                var LastUserDetails = await dbContext.UsersDetails
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId)
                    .OrderByDescending(q => q.CreateDate)
                    .FirstOrDefaultAsync();

                Dictionary<string, string> Translator2 = new Dictionary<string, string>();
                Translator2.Add(Core.Database.Types.ActivityLevelType.Small.ToString(), "Mała aktywność");
                Translator2.Add(Core.Database.Types.ActivityLevelType.Medium.ToString(), "Średnia aktywność");
                Translator2.Add(Core.Database.Types.ActivityLevelType.Large.ToString(), "Duża aktywność");
                Translator2.Add(Core.Database.Types.ActivityLevelType.ExtraLarge.ToString(), "Bardzo duża aktywność");

                Model.MeasurementAddModel model = new Model.MeasurementAddModel();

                var types = new List<SelectListItem>();

                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Small.ToString()], Value = Core.Database.Types.ActivityLevelType.Small.ToString()
                    //, Selected = LastUserDetails.ActivityLevel == Core.Database.Types.ActivityLevelType.Small ? true : false 
                });
                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Medium.ToString()], Value = Core.Database.Types.ActivityLevelType.Medium.ToString()
                    //,Selected = LastUserDetails.ActivityLevel == Core.Database.Types.ActivityLevelType.Medium ? true : false
                });
                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Large.ToString()], Value = Core.Database.Types.ActivityLevelType.Large.ToString()
                    //,Selected = LastUserDetails.ActivityLevel == Core.Database.Types.ActivityLevelType.Large ? true : false
                });
                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.ExtraLarge.ToString()], Value = Core.Database.Types.ActivityLevelType.ExtraLarge.ToString()
                    //,Selected = LastUserDetails.ActivityLevel == Core.Database.Types.ActivityLevelType.ExtraLarge ? true : false
                });

                model.ActivityLevels = new SelectList(types, "Value", "Text");

                Dictionary<Core.Database.Types.GenderType, string> Translator3 = new Dictionary<Core.Database.Types.GenderType, string>();

                Translator3.Add(Core.Database.Types.GenderType.Woman, "Kobieta");
                Translator3.Add(Core.Database.Types.GenderType.Man, "Mężczyzna");

                var types1 = new List<SelectListItem>();

                types1.Add(new SelectListItem() { Text = Translator3[Core.Database.Types.GenderType.Woman], Value = Core.Database.Types.GenderType.Woman.ToString()
                    //,Selected = LastUserDetails.Gender == Core.Database.Types.GenderType.Woman ? true : false
                });
                types1.Add(new SelectListItem() { Text = Translator3[Core.Database.Types.GenderType.Man], Value = Core.Database.Types.GenderType.Man.ToString()
                    //,Selected = LastUserDetails.Gender == Core.Database.Types.GenderType.Man ? true : false
                });

                model.Genders = new SelectList(types1, "Value", "Text");
                model.Age = LastUserDetails.Age;
                model.Height = LastUserDetails.Height;
                model.Weight = LastUserDetails.Weight;
                model.ActivityLevel = LastUserDetails.ActivityLevel.ToString();
                model.ArmCircumference = LastUserDetails.ArmCircumference;
                model.CalfCircumference = LastUserDetails.CalfCircumference;
                model.ChestCircumference = LastUserDetails.ChestCircumference;
                model.ForearmCircumference = LastUserDetails.ForearmCircumference;
                model.HipCircumference = LastUserDetails.HipCircumference;
                model.ThighCircumference = LastUserDetails.ThighCircumference;
                model.WaistCircumference = LastUserDetails.WaistCircumference;
                model.Gender = LastUserDetails.Gender;
                return View(model);
            }
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
                        var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                        var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                        var UserId = await dbContext.Users
                            .Where(q => EF.Functions.Like(q.Login, UserLogin))
                            .Where(q => q.IsActive && !q.IsDeleted)
                            .Select(q => q.Id).FirstOrDefaultAsync();

                        UserDetails userDetails = new UserDetails();

                        var LastMacro = await dbContext.UsersDetails
                            .Where(q => q.IsActive && !q.IsDeleted)
                            .Where(q => q.UserId == UserId)
                            .OrderByDescending(q => q.CreateDate)
                            .FirstOrDefaultAsync();

                        if(LastMacro != null)
                           userDetails = UserDetailsDto.Copy(LastMacro);

                        Core.Database.Types.ActivityLevelType activityLevel = Core.Database.Types.ActivityLevelType.Small;
                        if(Enum.TryParse<Core.Database.Types.ActivityLevelType>(model.ActivityLevel,out activityLevel))
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
                        userDetails.Gender = model.Gender;
                        userDetails.CreateDate = DateTime.Now;
                        userDetails.UserId = UserId;

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