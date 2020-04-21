using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HealthyService.WebPanel.Areas.Training.Controllers
{   [Area("Training")]
    public class HomeController : Controller
    {
        public IActionResult TrainingFBW()
        {
            return View();
        }
        public IActionResult TrainingSplit()
        {
            return View();
        }
        public IActionResult PrimaryTraining()
        {
            return View();
        }
         public IActionResult Calisthenics()
        {
            return View();
        }

    }
}