using Application.Interfaces;
using Domain.Entities;
using Application.DTOs;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        #region Get Services
        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.AddOrderAsync(order);
        }
        public async Task AddOrderItemAsync(OrderItem orderitem)
        {
            await _orderRepository.AddOrderItemAsync(orderitem);
        }
        #endregion
        #region Get Services
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
        public async Task<OrderListDto> GetOrderItemsAsync(int OrderId)
        {
            return await _orderRepository.GetOrderItemsAsync(OrderId);
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
        {
            return await _orderRepository.GetAllProductAsync();
        }
        public async Task<ProductDto> GetProductAsync(int ProductId)
        {
            return await _orderRepository.GetProductAsync(ProductId);
        }
        public async Task<List<OrderItemDto>> GetLastOpenOrderItemsAsync(int CustomerId)
        {
            return await _orderRepository.GetLastOpenOrderItemsAsync(CustomerId);
        }
        public async Task<List<OrderListGroupDto>> GetOrderItemsGroupAsync(int Customerid)
        {
            return await _orderRepository.GetOrderItemsGroupAsync(Customerid);
        }
        #endregion
        #region Delete Services
        public async Task DeleteOrderItems(int OrderId, int ProductId)
        {
            await _orderRepository.DeleteOrderItems(OrderId, ProductId);
        }
        #endregion
        #region Update Services
        public async Task CommitOrder(int OrderId)
        {
            await _orderRepository.CommitOrder(OrderId);
        }
        #endregion
    }
}
