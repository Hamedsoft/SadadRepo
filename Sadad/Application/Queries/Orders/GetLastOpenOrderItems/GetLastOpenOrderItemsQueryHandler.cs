using Application.Interfaces;
using Application.Queries.Orders.GetOrderById;
using MediatR;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Orders.GetLastOpenOrderItems
{
    public class GetLastOpenOrderItemsQueryHandler : IRequestHandler<GetLastOpenOrderItemsQuery, List<OrderItemDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetLastOpenOrderItemsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<List<OrderItemDto>> Handle(GetLastOpenOrderItemsQuery request, CancellationToken cancellationToken)
        {
            var Result = await _orderRepository.GetLastOpenOrderItemsAsync(request.CustomerId);
            if (Result == null)
                return null;
            return Result;
        }
    }
}
