using HealthyService.Core.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthyService.Core.Database
{
    public class DatabaseBus
    {
        public static void DeleteDatabase()
        {
            using(var db = new HealthyServiceContext())
            {
                db.Database.EnsureDeleted();
            }
        }
        public static void MigrateDatabase()
        {
            using(var db = new HealthyServiceContext())
            {
                db.Database.Migrate();
            }
        }
    }   


}
