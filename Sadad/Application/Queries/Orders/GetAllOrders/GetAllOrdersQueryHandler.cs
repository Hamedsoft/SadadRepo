using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Queries.Orders.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetAllOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var Orders = await _orderRepository.GetAllOrdersAsync();
            if (Orders == null)
                return null;
            return Orders;
        }
    }
}
