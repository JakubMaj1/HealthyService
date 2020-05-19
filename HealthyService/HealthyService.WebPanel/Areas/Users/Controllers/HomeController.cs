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
            MacroKcalAddModel model = new MacroKcalAddModel();

            using (var dbContext = new Core.Database.HealthyServiceContext())
            {

                var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();

                var LastUserDetails = await dbContext.UsersDetails
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId)
                    .OrderByDescending(q => q.CreateDate)
                    .FirstOrDefaultAsync();

                if(LastUserDetails != null)
                {
                    model.UserCarboLevel = LastUserDetails.UserCarboLevel != null ? LastUserDetails.UserCarboLevel.Value : 0;
                    model.UserFatLevel = LastUserDetails.UserFatLevel != null ? LastUserDetails.UserFatLevel.Value : 0;
                    model.UserProteinLevel = LastUserDetails.UserProteinLevel != null ? LastUserDetails.UserProteinLevel.Value :0;
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

                    var LastMacro = await dbContext.UsersDetails.Include(q=>q.User)// Dostęp do właściwości uzytkownika 
                        .Where(q => EF.Functions.Like(q.User.Login, UserLogin))
                        .Where(q => q.IsActive && !q.IsDeleted)
                        .OrderByDescending(q => q.CreateDate).FirstOrDefaultAsync();
    
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

                var FirstUserDetails = await dbContext.UsersDetails
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId)
                    .OrderBy(q => q.CreateDate)
                    .FirstOrDefaultAsync();

                Dictionary<string, string> Translator2 = new Dictionary<string, string>();
                Translator2.Add(Core.Database.Types.ActivityLevelType.Small.ToString(), "Mała aktywność");
                Translator2.Add(Core.Database.Types.ActivityLevelType.Medium.ToString(), "Średnia aktywność");
                Translator2.Add(Core.Database.Types.ActivityLevelType.Large.ToString(), "Duża aktywność");
                Translator2.Add(Core.Database.Types.ActivityLevelType.ExtraLarge.ToString(), "Bardzo duża aktywność");

                Model.MeasurementAddEditModel model = new Model.MeasurementAddEditModel();

                var types = new List<SelectListItem>();

                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Small.ToString()], Value = Core.Database.Types.ActivityLevelType.Small.ToString()
                });
                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Medium.ToString()], Value = Core.Database.Types.ActivityLevelType.Medium.ToString()
                });
                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.Large.ToString()], Value = Core.Database.Types.ActivityLevelType.Large.ToString()
                });
                types.Add(new SelectListItem() { Text = Translator2[Core.Database.Types.ActivityLevelType.ExtraLarge.ToString()], Value = Core.Database.Types.ActivityLevelType.ExtraLarge.ToString()
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
                if(FirstUserDetails != null)
                {
                    model.Age = FirstUserDetails.Age;
                    model.Height = FirstUserDetails.Height;
                    model.Weight = FirstUserDetails.Weight;
                    model.ActivityLevel = FirstUserDetails.ActivityLevel.ToString();
                    model.ArmCircumference = FirstUserDetails.ArmCircumference;
                    model.CalfCircumference = FirstUserDetails.CalfCircumference;
                    model.ChestCircumference = FirstUserDetails.ChestCircumference;
                    model.ForearmCircumference = FirstUserDetails.ForearmCircumference;
                    model.HipCircumference = FirstUserDetails.HipCircumference;
                    model.ThighCircumference = FirstUserDetails.ThighCircumference;
                    model.WaistCircumference = FirstUserDetails.WaistCircumference;
                    model.Gender = FirstUserDetails.Gender;
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> MeasurementAdd(MeasurementAddEditModel model)
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



                        var FirstMacro = await dbContext.UsersDetails
                            .Where(q => q.IsActive && !q.IsDeleted)
                            .Where(q => q.UserId == UserId)
                            .Where(q=> q.Age != null)
                            .OrderByDescending(q => q.CreateDate)
                            .LastOrDefaultAsync();

                        var LastMacro = await dbContext.UsersDetails
                            .Where(q => q.IsActive && !q.IsDeleted)
                            .Where(q => q.UserId == UserId)
                            .OrderByDescending(q => q.CreateDate)
                            .FirstOrDefaultAsync();

                        if (LastMacro != null)
                            userDetails = UserDetailsDto.Copy(LastMacro);

                        if (FirstMacro != null && FirstMacro.Age != null)
                        {

                            Core.Database.Types.ActivityLevelType activityLevel = Core.Database.Types.ActivityLevelType.Small;
                            if (Enum.TryParse<Core.Database.Types.ActivityLevelType>(model.ActivityLevel, out activityLevel))
                            {

                            }
                            FirstMacro.Age = model.Age;
                            FirstMacro.Height = model.Height;
                            FirstMacro.Weight = model.Weight;
                            FirstMacro.WaistCircumference = model.WaistCircumference;
                            FirstMacro.ActivityLevel = activityLevel;
                            FirstMacro.ArmCircumference = model.ArmCircumference;
                            FirstMacro.CalfCircumference = model.CalfCircumference;
                            FirstMacro.ChestCircumference = model.ChestCircumference;
                            FirstMacro.HipCircumference = model.HipCircumference;
                            FirstMacro.ThighCircumference = model.ThighCircumference;
                            FirstMacro.ForearmCircumference = model.ForearmCircumference;
                            FirstMacro.Gender = model.Gender;
                            FirstMacro.CreateDate = DateTime.Now;
                            FirstMacro.UserId = UserId;
                            await dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            Core.Database.Types.ActivityLevelType activityLevel = Core.Database.Types.ActivityLevelType.Small;
                            if (Enum.TryParse<Core.Database.Types.ActivityLevelType>(model.ActivityLevel, out activityLevel))
                            {

                            }
                            userDetails.Age = model.Age;
                            userDetails.Gender = model.Gender;
                            userDetails.Height = model.Height;
                            userDetails.Weight = model.Weight;
                            userDetails.WaistCircumference = model.WaistCircumference;
                            userDetails.ActivityLevel = activityLevel;
                            userDetails.ArmCircumference = model.ArmCircumference;
                            userDetails.CalfCircumference = model.CalfCircumference;
                            userDetails.ChestCircumference = model.ChestCircumference;
                            userDetails.HipCircumference = model.HipCircumference;
                            userDetails.ThighCircumference = model.ThighCircumference;
                            userDetails.ForearmCircumference = model.ForearmCircumference;
                            userDetails.CreateDate = DateTime.Now;
                            userDetails.UserId = UserId;

                            await dbContext.UsersDetails.AddAsync(userDetails);

                            await dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();

                        }
                    }
                }
            }
       
            else
            {

            }
            return RedirectToAction("Index", "Home", new { Area = "Dashboard" });
        }
        public async Task<IActionResult> MeasurementEdit()
        {
            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();

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

                Model.MeasurementAddEditModel model = new Model.MeasurementAddEditModel();

                var types = new List<SelectListItem>();

                types.Add(new SelectListItem()
                {
                    Text = Translator2[Core.Database.Types.ActivityLevelType.Small.ToString()],
                    Value = Core.Database.Types.ActivityLevelType.Small.ToString()
                });
                types.Add(new SelectListItem()
                {
                    Text = Translator2[Core.Database.Types.ActivityLevelType.Medium.ToString()],
                    Value = Core.Database.Types.ActivityLevelType.Medium.ToString()
                });
                types.Add(new SelectListItem()
                {
                    Text = Translator2[Core.Database.Types.ActivityLevelType.Large.ToString()],
                    Value = Core.Database.Types.ActivityLevelType.Large.ToString()
                });
                types.Add(new SelectListItem()
                {
                    Text = Translator2[Core.Database.Types.ActivityLevelType.ExtraLarge.ToString()],
                    Value = Core.Database.Types.ActivityLevelType.ExtraLarge.ToString()
                });






                model.ActivityLevels = new SelectList(types, "Value", "Text");
                if (LastUserDetails != null)
                {
                    model.Age = LastUserDetails.Age;
                    model.Gender = LastUserDetails.Gender;
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
                }


                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> MeasurementEdit(MeasurementAddEditModel model)
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

                        if (LastMacro != null)
                            userDetails = UserDetailsDto.Copy(LastMacro);

                        Core.Database.Types.ActivityLevelType activityLevel = Core.Database.Types.ActivityLevelType.Small;
                        if (Enum.TryParse<Core.Database.Types.ActivityLevelType>(model.ActivityLevel, out activityLevel))
                        {

                        }
                        userDetails.Age = model.Age;
                        userDetails.Gender = model.Gender;
                        userDetails.Height = model.Height;
                        userDetails.Weight = model.Weight;
                        userDetails.WaistCircumference = model.WaistCircumference;
                        userDetails.ActivityLevel = activityLevel;
                        userDetails.ArmCircumference = model.ArmCircumference;
                        userDetails.CalfCircumference = model.CalfCircumference;
                        userDetails.ChestCircumference = model.ChestCircumference;
                        userDetails.HipCircumference = model.HipCircumference;
                        userDetails.ThighCircumference = model.ThighCircumference;
                        userDetails.ForearmCircumference = model.ForearmCircumference;
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


        public async Task<IActionResult> GetUserDetails()
        {
            var userNameIdentifierClaim = this.HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var UserLogin = userNameIdentifierClaim != null ? userNameIdentifierClaim.Value : null;

            using (var dbContext = new Core.Database.HealthyServiceContext())
            {
                var UserId = await dbContext.Users
                    .Where(q => EF.Functions.Like(q.Login, UserLogin))
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Select(q => q.Id).FirstOrDefaultAsync();

                var lastUserDetails = await dbContext.UsersDetails
                    .Where(q => q.IsActive && !q.IsDeleted)
                    .Where(q => q.UserId == UserId)
                    .OrderByDescending(q => q.CreateDate)
                    .FirstOrDefaultAsync();

                string jsonValue = Newtonsoft.Json.JsonConvert.SerializeObject(lastUserDetails);

                return Json(jsonValue);
            }
        }
    }
}