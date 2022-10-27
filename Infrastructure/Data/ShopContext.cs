using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }

        public DbSet<Product>Products {get; set;}
        public DbSet<ProductBrand> ProductBrands{ get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            model.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var item in model.Model.GetEntityTypes())
                {
                    var properties = item.ClrType.GetProperties().Where(p=>p.PropertyType == typeof(decimal));

                    var dateTime = item.ClrType.GetProperties().Where(p=>p.PropertyType ==typeof(DateTimeOffset));

                    foreach(var property in dateTime)
                    {
                        model.Entity(item.Name).Property(property.Name).HasConversion(new DateTimeOffsetToBinaryConverter());
                    }

                     foreach (var property in properties)
                    {
                        model.Entity(item.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
        }
        
    }
}