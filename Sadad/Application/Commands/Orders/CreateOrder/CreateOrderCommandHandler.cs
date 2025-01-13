using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                Customer = request.Customer,
                SubTotal = request.SubTotal,
                Status = request.Status
            };
            await _orderRepository.AddOrderAsync(order);
            return order;
        }
    }

}
