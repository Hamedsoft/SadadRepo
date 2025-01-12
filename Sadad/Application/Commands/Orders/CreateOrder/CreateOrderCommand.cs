using MediatR;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public int Customer { get; set; }
        public double SubTotal { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
