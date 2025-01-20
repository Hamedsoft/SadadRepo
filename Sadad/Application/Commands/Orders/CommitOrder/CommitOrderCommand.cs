using MediatR;

namespace Application.Commands.Orders.CommitOrder
{
    public class CommitOrderCommand : IRequest
    {
        public int OrderId { get; set; }
        public CommitOrderCommand(int orderId)
        {
            OrderId = orderId;
        }
    }
}
