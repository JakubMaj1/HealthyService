using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyService.WebPanel.Areas.Users.Model
{
    public class MacroKcalAddModel
    {
       
        public long UserDemendLevel { get; set; }
        [Range(0, 100000)]
        [Required]
        public long UserProteinLevel { get; set;}
        [Range(0, 100000)]

        [Required]

        public long UserCarboLevel { get; set;}
        [Range(0, 100000)]
        [Required]

        public long UserFatLevel { get; set;}


        //kalorie
        //Białko
        //Wegle
        //tłuszcze
    }
}
