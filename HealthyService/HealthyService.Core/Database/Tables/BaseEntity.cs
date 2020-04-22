using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
    public class BaseEntity
    {
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }

        public BaseEntity()
        {
            CreateDate = DateTime.Now;
     
            IsActive = true;

            IsDeleted = false;
        }
    }

}
