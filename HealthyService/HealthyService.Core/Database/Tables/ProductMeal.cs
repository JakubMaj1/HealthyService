using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
   public class ProductMeal
    {
        public long MealId { get; set; }
        public Product Product { get; set; }

        public long ProductId { get; set; }
        public Meal Meal { get; set; }
    }
}
