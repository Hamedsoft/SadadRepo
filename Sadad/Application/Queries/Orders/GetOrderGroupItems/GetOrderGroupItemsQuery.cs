using Domain.Entities;
using MediatR;
using Shared.DTOs;

namespace Application.Queries.Orders.GetOrderGroupItems
{
    public class GetOrderGroupItemsQuery : IRequest<List<OrderListGroupDto>>
    {
        public int CustomerId { get; set; }
        public GetOrderGroupItemsQuery(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
