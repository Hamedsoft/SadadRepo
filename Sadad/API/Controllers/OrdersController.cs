using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using Domain.Entities;
using Application.Queries.Orders.GetOrderById;
using MediatR;
using Application.Interfaces;
using Application.Queries.Orders.GetAllOrders;
using Application.Queries.Orders.GetOrderGroupItems;
using Application.Queries.Orders.GetProducts;
using Application.Queries.Orders.GetLastOpenOrderItems;
using Application.Commands.Orders.AddOrder;
using Application.Commands.Orders.AddOrderItem;
using Application.Commands.Orders.RemoveOrderItems;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMediator _mediator;
        public OrdersController(IOrderService orderService, IMediator mediator)
        {
            _orderService = orderService;
            _mediator = mediator;
        }
        #region Post Methods

        [HttpPost]
        [Route("AddOrderNormal")] //Remove This
        public async Task<IActionResult> AddOrderNormal([FromBody] OrderDto order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order data.");
            }
            var newOrder = new Order
            {
                Customer = order.Customer,
                SubTotal = order.SubTotal,
                Status = order.Status
            };
            await _orderService.AddOrderAsync(newOrder);
            return Ok(newOrder);
        }

        [HttpPost]
        [Route("AddOrder")] //Fixed for CQRS
        public async Task<IActionResult> AddOrder([FromBody] OrderDto order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order data.");
            }
            var command = new AddOrderCommand(customer: order.Customer, subTotal: order.SubTotal, status: order.Status);
            Order Model = await _mediator.Send(command);
            return Ok(Model);
        }

        [HttpPost]
        [Route("AddOrderItem")]  //Fixed for CQRS
        public async Task<IActionResult> AddOrderItemAsync([FromBody] AddOrderItemDto orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest("Invalid order data.");
            }
            var command = new AddOrderItemCommand(customerId : orderItem.CustomerId, quantity : 1, productId : orderItem.ProductId);
            int DraftOrderItems = await _mediator.Send(command);
            return Ok(DraftOrderItems);
        }
        #endregion
        #region Get Methods

        [HttpGet]
        [Route("GetOrdersNormal")]  //Remove This
        public async Task<IActionResult> GetOrdersNormal()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet]
        [Route("GetOrders")] //Fixed for CQRS
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetAllOrdersQuery();  // ساختن یک Query
            var orders = await _mediator.Send(query);  // ارسال Query از طریق MediatR
            return Ok(orders);
        }

        [HttpGet]
        [Route("GetOrderItems/{Order}")] //Fixed for CQRS
        public async Task<IActionResult> GetOrdersCQRS(int Order = 1)
        {
            var query = new GetOrderByIdQuery(Order);  // ساختن یک Query
            var order = await _mediator.Send(query);  // ارسال Query از طریق MediatR

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        [Route("GetOrderItemsNormal/{Order}")] //Remove This
        public async Task<IActionResult> GetOrderItemsNormal(int Order = 1)
        {
            var OrderItems = await _orderService.GetOrderItemsAsync(Order);
            return Ok(OrderItems);
        }

        [HttpGet]
        [Route("GetOrderGroupItemsNormal/{CustomerId}")] //Remove This
        public async Task<IActionResult> GetOrderGroupItemsNormal(int CustomerId = 1)
        {
            var OrderItems = await _orderService.GetOrderItemsGroupAsync(CustomerId);
            return Ok(OrderItems);
        }

        [HttpGet]
        [Route("GetOrderGroupItems/{CustomerId}")] //Fixed for CQRS
        public async Task<IActionResult> GetOrderGroupItems(int CustomerId = 1)
        {
            var query = new GetOrderGroupItemsQuery(CustomerId);  // ساختن یک Query
            var response = await _mediator.Send(query);  // ارسال Query از طریق MediatR

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        [Route("GetProductsNormal")] //Remove This
        public async Task<IActionResult> GetProductsNormal()
        {
            var products = await _orderService.GetAllProductAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("GetProducts")] //Fixed for CQRS
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetProductsQuery();  // ساختن یک Query
            var response = await _mediator.Send(query);  // ارسال Query از طریق MediatR

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet] 
        [Route("GetLastOpenOrderItemsNormal/{CustomerId}")] //Remove This
        public async Task<IActionResult> GetLastOpenOrderItemsNormal(int CustomerId = 1)
        {
            var products = await _orderService.GetLastOpenOrderItemsAsync(CustomerId);
            return Ok(products);
        }

        [HttpGet]
        [Route("GetLastOpenOrderItems/{CustomerId}")] //Fixed for CQRS
        public async Task<IActionResult> GetLastOpenOrderItems(int CustomerId = 1)
        {
            var query = new GetLastOpenOrderItemsQuery(CustomerId);  // ساختن یک Query
            var response = await _mediator.Send(query);  // ارسال Query از طریق MediatR

            if (response == null)
                return NotFound();

            return Ok(response);
        }
        #endregion
        #region Delete Methods

        [HttpDelete]
        [Route("RemoveOrderItems")] //Fixed for CQRS
        public async Task<IActionResult> RemoveOrderItemAsync([FromBody] DeleteOrderItemDto orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest("Invalid order data.");
            }

            int OrderId = orderItem.OrderId;
            int ProductId = orderItem.ProductId;

            var query = new RemoveOrderItemsCommand(OrderId, ProductId);  // ساختن یک Query
            await _mediator.Send(query);  // ارسال Query از طریق MediatR
            return Ok();
        }
        #endregion
        #region Update Methods

        [HttpPut]
        [Route("CommitOrder")]
        public async Task<IActionResult> CommitOrder([FromBody] CommitOrderDto orderInfo)
        {
            if (orderInfo == null)
            {
                return BadRequest("Invalid order data.");
            }
            await _orderService.CommitOrder(orderInfo.OrderId);
            return Ok();
        }
        #endregion
    }
}
