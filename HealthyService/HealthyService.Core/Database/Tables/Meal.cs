using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
    public class Meal : BaseEntity
    {
        public long Id { get; set; }
        public DateTime MealDateTime { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public long UserId { get; set; }
        public List<ProductMeal> Products { get; set; }
    }
}
