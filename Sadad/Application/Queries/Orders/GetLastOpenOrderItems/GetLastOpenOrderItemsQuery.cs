using MediatR;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Orders.GetLastOpenOrderItems
{
    public class GetLastOpenOrderItemsQuery : IRequest<List<OrderItemDto>>
    {
        public int CustomerId { get; set; }
        public GetLastOpenOrderItemsQuery(int customerId)
        {
            CustomerId = customerId;
        }

    }
}
