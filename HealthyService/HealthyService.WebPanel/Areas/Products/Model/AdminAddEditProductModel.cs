using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HealthyService.WebPanel.Areas.Products.Model
{
    public class AdminAddEditProductModel
    {
        [MaxLength(15)]
        [Required(ErrorMessage = "Podaj nazwę produktu")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Podaj ilość białka")]
        public decimal Protein { get; set; }
        [Required(ErrorMessage = "Podaj ilość węglowodanów")]
        public decimal Carbo { get; set; }
        [Required(ErrorMessage = "Podaj ilość tłuszczy")]
        public decimal Fat { get; set; }
        [Required(ErrorMessage = "Wybierz")]

        public Core.Database.Types.ProductMeasureType ProductMeasure { get; set; }
        public SelectList ProductMeasures { get; set; }

    }
}
