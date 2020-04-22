﻿using HealthyService.Core.Database.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
    public class Product : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public float Protein { get; set; }

        public float Carbo { get; set; }

        public float Fat { get; set; }

        public ProductMeasureType ProductMeasure { get; set; }
    }
}