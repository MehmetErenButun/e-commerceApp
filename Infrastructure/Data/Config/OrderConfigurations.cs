using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(x=>x.ShipToAddress, i=>{
                i.WithOwner();
            });

            builder.Property(x=>x.Status).HasConversion(i=>i.ToString(),i=>(OrderStatus) Enum.Parse(typeof(OrderStatus),i));

            builder.HasMany(o=>o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}