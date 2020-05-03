using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyService.WebPanel.Areas.Users.Model
{
    public class MacroKcalAddModel
    {
        [Required(ErrorMessage ="Podaj zapotrzebowanie") ]
        public long UserDemendLevel { get; set;}
        [Required(ErrorMessage = "Podaj Białko")]

        public long? UserProteinLevel { get; set;}
        [Required(ErrorMessage = "Podaj Węglowodany")]

        public long? UserCarboLevel { get; set;}
        [Required(ErrorMessage = "Podaj Tłuszcze")]

        public long? UserFatLevel { get; set;}


        //kalorie
        //Białko
        //Wegle
        //tłuszcze
    }
}
