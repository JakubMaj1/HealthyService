using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
    public class Meal :BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        
    }
}
