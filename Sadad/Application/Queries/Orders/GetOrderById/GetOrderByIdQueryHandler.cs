using Application.Interfaces;
using MediatR;
using Application.DTOs;


namespace Application.Queries.Orders.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderListDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderListDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var sorder = await _orderRepository.GetOrderItemsAsync(request.Id);
            if (sorder == null)
                return null;

            return new OrderListDto
            {
                Order = sorder.Order,
                OrderItem = sorder.OrderItem
            };
        }

    }

}
