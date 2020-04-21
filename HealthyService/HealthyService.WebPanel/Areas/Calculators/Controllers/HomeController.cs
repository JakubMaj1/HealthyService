using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HealthyService.WebPanel.Areas.Calculators.Controllers
{
    [Area("Calculators")]
    public class HomeController : Controller
    {
        public IActionResult BMI()
        {
            return View();
        }
        public IActionResult BMR()
        {
            return View();
        }
        public IActionResult Lift()
        {
            return View();
        }
        public IActionResult PrimaryCalculators()
        {
            return View();
        }

    }
}