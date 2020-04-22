using HealthyService.Core.Database.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
    public class UserDetails : BaseEntity
    {
        public long Id { get; set; }
        public long? Weight { get; set; }

        public long? Height { get; set; }
        public long? Age { get; set; }
        public ActivityLevelType ActivityLevel { get; set; }

        public GenderType Gender { get; set; }

        public long? UserProteinLevel { get; set; }

        public long? UserCarboLevel { get; set; }

        public long? UserFatLevel { get; set; }

        public long? UserDemendLevel { get; set; }

        public long? WaistCircumference { get; set; }

        public long? HipCircumference { get; set; }

        public long? ChestCircumference { get; set; }

        public long? CalfCircumference { get; set; }

        public long? ThighCircumference { get; set; }

        public long? ArmCircumference { get; set; }

        public long? ForearmCircumference { get; set; }
    }
}
