using HealthyService.Core.Database.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HealthyService.Core.Database
{
    public class StartData
    {
        public async Task AddDataFromCodeAsync()
        {
            using (var dbContext = new HealthyServiceContext())
            {
                using (var transaction = await dbContext.Database.BeginTransactionAsync())
                {
                    var product1 = new Product()
                    {
                        Name = "Pierś z kurczaka",
                        Protein = 22,
                        Carbo = 0,
                        Fat = 3,
                        ProductMeasure = Types.ProductMeasureType.Gram
                    };

                    var product2 = new Product()
                    {
                        Name = "Ryż biały",
                        Protein = 6,
                        Carbo = 83,
                        Fat = 0,
                        ProductMeasure = Types.ProductMeasureType.Gram
                    };

                    var product3 = new Product()
                    {
                        Name = "Makaron",
                        Protein = 13,
                        Carbo = 70,
                        ProductMeasure = Types.ProductMeasureType.Gram
                    };
                    var product4 = new Product()
                    {
                        Name = "Jajo",
                        Protein = 12,
                        Carbo = 1,
                        ProductMeasure = Types.ProductMeasureType.Piece,
                    };
                    var product5 = new Product()
                    {
                        Name = "Mleko 3,2%",
                        Protein = 3,
                        Carbo = 3,
                        ProductMeasure = Types.ProductMeasureType.Gram,
                        Fat = 5
                    };
                    await dbContext.Products.AddRangeAsync(new Product[] { product1, product2, product3, product4, product5 });

                    await dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
            }
        }
    }
}
