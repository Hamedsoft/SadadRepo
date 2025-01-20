using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Orders.AddOrderItem
{
    public class AddOrderItemCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public AddOrderItemCommand(int customerId, int quantity, int productId)
        {
            CustomerId = customerId;
            Quantity = quantity;
            ProductId = productId;
        }
    }
}
