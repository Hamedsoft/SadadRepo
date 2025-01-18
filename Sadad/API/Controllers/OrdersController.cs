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
using Application.Exceptions;
using Domain.Enums;

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
                throw new CustomException(ErrorCode.ModelIsNull);
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
                throw new CustomException(ErrorCode.ModelIsNull);
            }
            var command = new AddOrderItemCommand(customerId: orderItem.CustomerId, quantity: 1, productId: orderItem.ProductId);
            int DraftOrderItems = await _mediator.Send(command);
            return Ok(DraftOrderItems);
        }
        #endregion
        #region Get Methods

        [HttpGet]
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetAllOrdersQuery();
            var orders = await _mediator.Send(query);
            if (orders == null)
                throw new CustomException(ErrorCode.ModelIsNull);
            return Ok(orders);
        }

        [HttpGet]
        [Route("GetOrderItems/{Order}")]
        public async Task<IActionResult> GetOrdersCQRS(int Order = 1)
        {
            var query = new GetOrderByIdQuery(Order);
            var order = await _mediator.Send(query);

            if (order == null)
                throw new CustomException(ErrorCode.ModelIsNull);

            return Ok(order);
        }

        [HttpGet]
        [Route("GetOrderGroupItems/{CustomerId}")]
        public async Task<IActionResult> GetOrderGroupItems(int CustomerId = 1)
        {
            var query = new GetOrderGroupItemsQuery(CustomerId);
            var response = await _mediator.Send(query);

            if (response == null)
                throw new CustomException(ErrorCode.ModelIsNull);

            return Ok(response);
        }

        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetProductsQuery();
            var response = await _mediator.Send(query);

            if (response == null)
                throw new CustomException(ErrorCode.ModelIsNull);

            return Ok(response);
        }

        [HttpGet]
        [Route("GetLastOpenOrderItems/{CustomerId}")]
        public async Task<IActionResult> GetLastOpenOrderItems(int CustomerId = 1)
        {
            var query = new GetLastOpenOrderItemsQuery(CustomerId);
            var response = await _mediator.Send(query);

            if (response == null)
                throw new CustomException(ErrorCode.ModelIsNull);

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
                throw new CustomException(ErrorCode.ModelIsNull);
            }

            int OrderId = orderItem.OrderId;
            int ProductId = orderItem.ProductId;

            var query = new RemoveOrderItemsCommand(OrderId, ProductId);
            await _mediator.Send(query);
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
                throw new CustomException(ErrorCode.ModelIsNull);
            }

            var query = new CommitOrderCommand(orderInfo.OrderId);
            await _mediator.Send(query);
            return Ok();
        }
        #endregion
    }
}
