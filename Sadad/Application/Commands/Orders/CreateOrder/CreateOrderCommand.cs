using Domain.Entities;
using MediatR;
using Shared.DTOs;

namespace Application.Commands.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public int Customer { get; set; }
        public double SubTotal { get; set; }
        public int Status { get; set; }
        public CreateOrderCommand(int customer, double subTotal, int status)
        {
            Customer = customer;
            SubTotal = subTotal;
            Status = status;
        }
    }
}
