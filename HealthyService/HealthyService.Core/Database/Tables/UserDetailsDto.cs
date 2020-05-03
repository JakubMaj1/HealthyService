using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database.Tables
{
    public static class UserDetailsDto
    {
        public static UserDetails Copy(UserDetails source)
        {
            UserDetails userDetails = new UserDetails();

            userDetails.ActivityLevel = source.ActivityLevel;
            userDetails.Age = source.Age;
            userDetails.ArmCircumference = source.ArmCircumference;
            userDetails.CalfCircumference = source.CalfCircumference;
            userDetails.ChestCircumference = source.ChestCircumference;
            userDetails.CreateDate = source.CreateDate;
            userDetails.ForearmCircumference = source.ForearmCircumference;
            userDetails.Gender = source.Gender;
            userDetails.Height = source.Height;
            userDetails.UserCarboLevel = source.UserCarboLevel;
            userDetails.UserProteinLevel = source.UserProteinLevel;
            userDetails.UserFatLevel = source.UserFatLevel;
            userDetails.UserDemendLevel = source.UserDemendLevel;
            userDetails.HipCircumference = source.HipCircumference;
            userDetails.ThighCircumference = source.ThighCircumference;           
            userDetails.WaistCircumference = source.WaistCircumference;
            userDetails.Weight = source.Weight;

            return userDetails;
        }
    }
}
