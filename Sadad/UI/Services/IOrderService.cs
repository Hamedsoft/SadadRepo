using Shared.DTOs;

namespace UI.Services
{
    public interface IOrderService
    {
        Task<List<ProductDto>> GetProducts();
        Task<List<OrderItemDto>> GetLastOpenOrderItems(int customerId);
        Task<List<OrderListGroupDto>> GetOrderGroupItems(int customerId);
        Task<bool> CommitOrderAsync(int orderId);
        Task<bool> AddOrderItem(int customerId, int productId);
        Task<bool> RemoveOrderItem(int orderId, int productId);
    }
}
