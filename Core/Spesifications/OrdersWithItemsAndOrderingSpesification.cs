using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;

namespace Core.Spesifications
{
    public class OrdersWithItemsAndOrderingSpesification : BaseSpesification<Order>
    {
        public OrdersWithItemsAndOrderingSpesification(string email):base(o=>o.BuyerEmail==email)
        {
            AddInclude(o=>o.OrderItems);
            AddInclude(o=>o.DeliveryMethod);
            AddOrderByDescending(o=>o.OrderDate);
        }

        public OrdersWithItemsAndOrderingSpesification(int id,string email) : base(o=>o.Id==id && o.BuyerEmail==email)
        {
            AddInclude(o=>o.OrderItems);
            AddInclude(o=>o.DeliveryMethod);
        }
    }
}