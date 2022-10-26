using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basket;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _dRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        public OrderService(IGenericRepository<Order> orderRepo,IGenericRepository<DeliveryMethod> dRepo,IGenericRepository<Product> productRepo,IBasketRepository basket)
        {
            _orderRepo = orderRepo;
            _dRepo = dRepo;
            _productRepo = productRepo;
            _basket = basket;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAdress)
        {
            var basket =await  _basket.GetBasketAsync(basketId);

            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var producItem =await _productRepo.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(producItem.Id,producItem.Name,producItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered,producItem.Price,item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await _dRepo.GetByIdAsync(deliveryMethodId);

            var subtotal = items.Sum(x => x.Price * x.Quantity);

            var order = new Order(buyerEmail,shippingAdress,deliveryMethod,items,subtotal);

            //SAVE TODO

            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethods()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}