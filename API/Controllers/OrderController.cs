using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {   
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User.RetrieveEmail();
            var address = _mapper.Map<AdressDto,Address>(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email,orderDto.DeliveryMethodId,orderDto.BasketId,address);

            if(order==null) return BadRequest(new ApiResponse(400,"propblem"));

            return Ok(order);
        }

        [HttpGet]

        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmail();
            var orders = await _orderService.GetOrdersForUserAsync(email);
            var ordersToDto = _mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(ordersToDto);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<OrderToReturnDto>> GetOrderByUserId(int id)
        {
            var email = HttpContext.User.RetrieveEmail();

            var order = await _orderService.GetOrderByIdAsync(id,email);

            if(order==null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethods")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDelivery()
        {
            return Ok(await _orderService.GetDeliveryMethods());
        }
    }
}