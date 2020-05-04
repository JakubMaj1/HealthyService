using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyService.WebPanel.Areas.Products.Model
{
    public class AdminAddProductModel
    {
        [MaxLength(15)]
        [Required(ErrorMessage = "Podaj nazwę produktu")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Podaj ilość białka")]
        public float Protein { get; set; }
        [Required(ErrorMessage = "Podaj ilość węglowodanów")]
        public float Carbo { get; set; }
        [Required(ErrorMessage = "Podaj ilość tłuszczy")]
        public float Fat { get; set; }




    }
}
