using API.DTOs;
using API.Models;

namespace API.Repositories
{
    public interface IOrderRepository
    {
        #region Post Interfaces
        Task AddOrderAsync(Order order);
        Task AddOrderItemAsync(OrderItem orderitem);
        #endregion
        #region Get Interfaces
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<OrderListDto> GetOrderItemsAsync(int OrderId);
        Task<IEnumerable<ProductDto>> GetAllProductAsync();
        Task<ProductDto> GetProductAsync(int ProductId);
        Task<List<OrderItemDto>> GetLastOpenOrderItemsAsync(int CustomerId);
        Task<List<OrderListGroupDto>> GetOrderItemsGroupAsync(int Customerid);
        #endregion
        #region Delete Interfaces
        Task DeleteOrderItems(int OrderId, int ProductId);
        #endregion
        #region Update Interfaces
        Task CommitOrder(int OrderId);
        #endregion
    }
}
