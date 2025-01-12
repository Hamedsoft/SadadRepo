using MediatR;
using Shared.DTOs;

namespace Application.Queries.Orders.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<OrderListDto>
    {
        public int Id { get; set; }

        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
    }
}
