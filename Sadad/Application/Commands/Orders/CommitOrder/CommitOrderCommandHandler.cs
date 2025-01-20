
using Application.Commands.Orders.AddOrderItem;
using Application.Interfaces;
using MediatR;

namespace Application.Commands.Orders.CommitOrder
{
    internal class CommitOrderCommandHandler : IRequestHandler<CommitOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public CommitOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(CommitOrderCommand request, CancellationToken cancellationToken)
        {
            await _orderRepository.CommitOrder(OrderId: request.OrderId);
        }
    }
}
