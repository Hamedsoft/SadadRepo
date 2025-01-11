using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using API.DTOs;
using API.Models;
using API.Repositories;
using API.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
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
            var newOrder = new Order
            {
                Customer = order.Customer,
                SubTotal = order.SubTotal,
                Status = order.Status
            };
            await _orderService.AddOrderAsync(newOrder);
            return Ok(newOrder);
        }
        #region Get Methods
        [HttpGet]
        [Route("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }
        [HttpPost]
        [Route("AddOrderItem")]
        #endregion
        public async Task<IActionResult> AddOrderItemAsync([FromBody] AddOrderItemDto orderItem)
        {
            if (orderItem == null)
            {
                return BadRequest("Invalid order data.");
            }

            int OrderId;
            var LastDraftsOrder = await _orderService.GetLastOpenOrderItemsAsync(orderItem.CustomerId);
            if (LastDraftsOrder.Count() == 0)
            {
                var newOrder = new Order
                {
                    Customer = orderItem.CustomerId,
                    SubTotal = 0,
                    Status = 0
                };
                await _orderService.AddOrderAsync(newOrder);
                OrderId = newOrder.Id;
            }
            else
            {
                OrderId = LastDraftsOrder.Select(oi => oi.OrderId).FirstOrDefault();
            }
            ProductDto ProductInfo = await _orderService.GetProductAsync(orderItem.ProductId);
            OrderItem NewOrderItem = new OrderItem
            {
                OrderId = OrderId,
                Quantity = 1,
                Price = ProductInfo.Price,
                ProductId = orderItem.ProductId
            };
            await _orderService.AddOrderItemAsync(NewOrderItem);
            var DraftsOrderItems = await _orderService.GetLastOpenOrderItemsAsync(orderItem.CustomerId);
            return Ok(DraftsOrderItems.Count());
        }
        [HttpGet]
        [Route("GetOrderItems/{Order}")]
        public async Task<IActionResult> GetOrderItems(int Order = 1)
        {
            var OrderItems = await _orderService.GetOrderItemsAsync(Order);
            return Ok(OrderItems);
        }
        [HttpGet]
        [Route("GetOrderGroupItems/{CustomerId}")]
        public async Task<IActionResult> GetOrderGroupItems(int CustomerId = 1)
        {
            var OrderItems = await _orderService.GetOrderItemsGroupAsync(CustomerId);
            return Ok(OrderItems);
        }
        [HttpGet]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _orderService.GetAllProductAsync();
            return Ok(products);
        }
        [HttpGet]
        [Route("GetLastOpenOrderItems/{CustomerId}")]
        public async Task<IActionResult> GetLastOpenOrderItems(int CustomerId = 1)
        {
            var products = await _orderService.GetLastOpenOrderItemsAsync(CustomerId);
            return Ok(products);
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

            await _orderService.DeleteOrderItems(OrderId, ProductId);
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
