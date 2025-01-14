using Domain.Entities;
using MediatR;
using Application.DTOs;

namespace Application.Commands.Orders.AddOrder
{
    public class AddOrderCommand : IRequest<Order>
    {
        public int Customer { get; set; }
        public double SubTotal { get; set; }
        public int Status { get; set; }
        public AddOrderCommand(int customer, double subTotal, int status)
        {
            Customer = customer;
            SubTotal = subTotal;
            Status = status;
        }
    }
}
