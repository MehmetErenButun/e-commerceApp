using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class ShopContextSeed
    {
        public static async Task SeedAsync(ShopContext context,ILoggerFactory loggerFactory)
        {
            try
            {
                if(!context.ProductBrands.Any())
                {
                    var brandsData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
             catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ShopContextSeed>();
                logger.LogError(ex.Message);
            }
            try
            {
                if(!context.ProductTypes.Any())
                {
                    var typesData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                    var brands = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    foreach (var item in brands)
                    {
                        context.ProductTypes.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
             catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ShopContextSeed>();
                logger.LogError(ex.Message);
            }
            try
            {
                if(!context.Products.Any())
                {
                    var productsData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/products.json");

                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ShopContextSeed>();
                logger.LogError(ex.Message);
            }
             try
            {
                if(!context.DeliveryMethods.Any())
                {
                    var deliveryData =
                    File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");

                    var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                    foreach (var item in deliveries)
                    {
                        context.DeliveryMethods.Add(item);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ShopContextSeed>();
                logger.LogError(ex.Message);
            }
        }
        
    }
}