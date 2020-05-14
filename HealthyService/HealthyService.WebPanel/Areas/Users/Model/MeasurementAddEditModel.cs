using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyService.WebPanel.Areas.Users.Model
{
    public class MeasurementAddEditModel
    {
        [Range(1, 99, ErrorMessage = "Należy podać wiek z przedziału 1 - 99")]
        [Required(ErrorMessage ="dsada")]
        public long? Age { get; set; }
        [Range(1, 1000, ErrorMessage = "Podaj wagę w kilogramach")]
        public long? Weight { get; set; }
        [Range(1, 300, ErrorMessage = "Podaj wzrost w centymetrach")]
        public long? Height { get; set; }
        public Core.Database.Types.GenderType Gender { get; set; }

        public string ActivityLevel { get; set; }

        public SelectList Genders { get; set; }

        public SelectList ActivityLevels { get; set; }

        public long? WaistCircumference { get; set; }

        public long? HipCircumference { get; set; }

        public long? ChestCircumference { get; set; }

        public long? CalfCircumference { get; set; }

        public long? ThighCircumference { get; set; }

        public long? ArmCircumference { get; set; }

        public long? ForearmCircumference { get; set; }

    }
}
