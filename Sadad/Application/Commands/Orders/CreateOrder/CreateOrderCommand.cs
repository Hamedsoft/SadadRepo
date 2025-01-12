using MediatR;
using Shared.DTOs;

namespace Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public int Customer { get; set; }
        public double SubTotal { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
