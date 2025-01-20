
using Application.Commands.Orders.AddOrderItem;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Orders.RemoveOrderItems
{
    internal class RemoveOrderItemsCommandHandler : IRequestHandler<RemoveOrderItemsCommand>
    {
        private readonly IOrderRepository _orderRepository;

        public RemoveOrderItemsCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Handle(RemoveOrderItemsCommand request, CancellationToken cancellationToken)
        {
            await _orderRepository.DeleteOrderItems(OrderId : request.OrderId, ProductId : request.ProductId);
        }
    }
}
