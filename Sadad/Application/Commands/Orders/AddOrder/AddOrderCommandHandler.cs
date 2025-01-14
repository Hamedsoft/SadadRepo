using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Orders.AddOrder
{
    public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Order>
    {
        private readonly IOrderRepository _orderRepository;

        public AddOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<Order> Handle(AddOrderCommand request, CancellationToken cancellationToken)
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
