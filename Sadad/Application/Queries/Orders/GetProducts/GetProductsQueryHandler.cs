using Application.Interfaces;
using Application.Queries.Orders.GetOrderGroupItems;
using MediatR;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Orders.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IOrderRepository _orderRepository;
        public GetProductsQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var Result = await _orderRepository.GetAllProductAsync();
            if (Result == null)
                return null;
            return Result;
        }
    }
}
