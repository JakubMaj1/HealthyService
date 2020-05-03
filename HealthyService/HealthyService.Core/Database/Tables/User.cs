using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
    public class User : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string SureName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<UserDetails> UsersDetails { get; set; } // Dodane EF Relacje
        public List<Meal> Meals { get; set; }
        public List<Product> Products { get; set; }
    }
}
