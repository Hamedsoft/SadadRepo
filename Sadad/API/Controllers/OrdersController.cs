using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
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
using Application.Commands.Orders.CommitOrder;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        #region Post Methods

        [HttpPost]
        [Route("AddOrder")]
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
        [Route("AddOrderItem")] 
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
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetAllOrdersQuery();  // ساختن یک Query
            var orders = await _mediator.Send(query);  // ارسال Query از طریق MediatR
            return Ok(orders);
        }

        [HttpGet]
        [Route("GetOrderItems/{Order}")]
        public async Task<IActionResult> GetOrdersCQRS(int Order = 1)
        {
            var query = new GetOrderByIdQuery(Order);  // ساختن یک Query
            var order = await _mediator.Send(query);  // ارسال Query از طریق MediatR

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet]
        [Route("GetOrderGroupItems/{CustomerId}")]
        public async Task<IActionResult> GetOrderGroupItems(int CustomerId = 1)
        {
            var query = new GetOrderGroupItemsQuery(CustomerId);  // ساختن یک Query
            var response = await _mediator.Send(query);  // ارسال Query از طریق MediatR

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetProductsQuery();  // ساختن یک Query
            var response = await _mediator.Send(query);  // ارسال Query از طریق MediatR

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet]
        [Route("GetLastOpenOrderItems/{CustomerId}")]
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
        [Route("RemoveOrderItems")]
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

            var query = new CommitOrderCommand(orderInfo.OrderId);  // ساختن یک Query
            await _mediator.Send(query);  // ارسال Query از طریق MediatR
            return Ok();
        }
        #endregion
    }
}
