using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO
{
    public class OrderToReturnDto
    {   
        public string Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Core.Entities.OrderAggregate.Address ShipToAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderItems {get; set;}
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } 
    }
}