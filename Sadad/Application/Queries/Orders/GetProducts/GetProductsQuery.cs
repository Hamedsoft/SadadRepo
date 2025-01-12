using MediatR;
using Shared.DTOs;

namespace Application.Queries.Orders.GetProducts
{
    public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        public GetProductsQuery()
        {
        }
    }
}
