
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;



namespace HealthyService.WebPanel.Areas.Account.Model
{
    public class RegisterViewModel
    {
        [MaxLength(15)]
        [Required(ErrorMessage ="Podaj imię")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Email jest wymagany")]
        [DataType(DataType.EmailAddress)]
        [Remote("IsEmailValid", "Home", "Account", ErrorMessage = "Ten adres email jest użyty.", HttpMethod = "GET")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Podane Hasła nie są identyczne")]
        public string ConfirmPassword { get; set; }

        [MaxLength(25)]
        [Required(ErrorMessage = "Podaj nazwisko")]
        public string SureName { get; set; }

    }
}
