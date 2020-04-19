using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HealthyService.WebPanel.Areas.Account.Model
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage ="Wymagany email")]
        public string UserEmail { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage ="Wymagane hasło")]
        public string UserPassword { get; set; }
    }
}
