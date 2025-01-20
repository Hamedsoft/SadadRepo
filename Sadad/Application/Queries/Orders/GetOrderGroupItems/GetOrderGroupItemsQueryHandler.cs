using Application.Interfaces;
using Application.Queries.Orders.GetOrderById;
using MediatR;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Orders.GetOrderGroupItems
{
    public class GetOrderGroupItemsQueryHandler : IRequestHandler<GetOrderGroupItemsQuery, List<OrderListGroupDto>>
    {
        private readonly IOrderRepository _orderRepository;
        public GetOrderGroupItemsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<OrderListGroupDto>> Handle(GetOrderGroupItemsQuery request, CancellationToken cancellationToken)
        {
            var Result = await _orderRepository.GetOrderItemsGroupAsync(request.CustomerId);
            if (Result == null)
                return null;
            return Result;
        }
    }
}
