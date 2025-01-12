using Domain.Entities;
using MediatR;

namespace Application.Queries.Orders.GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<IEnumerable<Order>>
    {
        public GetAllOrdersQuery()
        {
        }
    }
}
