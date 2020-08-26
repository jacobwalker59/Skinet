using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Extensions;
using Core.Entities.OrderAggregate;
using API.Errors;
using System.Collections.Generic;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;

        }

        [HttpPost]
        public async Task<ActionResult<OrdersController>> CreateOrder(OrderDTO orderDTO)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var address = _mapper.Map<AddressDTO,Address>(orderDTO.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(email, orderDTO.DeliveryMethodId, orderDTO.BasketId, address);
            if(order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
            return Ok(order);

        }

        [HttpGet()]
        public async Task<ActionResult<IReadOnlyList<OrderDTO>>> GetOrdersForUser()
        {   
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var orders = await _orderService.GetOrdersForUserAsync(email);
            var x = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDTO>>(orders);
            return Ok(x);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromPrincipal();
            var order = await _orderService.GetOrderByIdAsync(id,email);
            if(order == null){
                return NotFound(new ApiResponse(404));
            }
            var x = _mapper.Map<Order,OrderToReturnDTO>(order);
            return x;

        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodAsync());
        }

    }
}