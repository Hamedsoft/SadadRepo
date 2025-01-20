
using MediatR;

namespace Application.Commands.Orders.RemoveOrderItems
{
    public class RemoveOrderItemsCommand : IRequest
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public RemoveOrderItemsCommand(int orderId, int productId)
        {
            ProductId = productId;
            OrderId = orderId;
        }
    }
}
