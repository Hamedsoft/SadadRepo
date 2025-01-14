using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.Orders.AddOrderItem
{
    public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public AddOrderItemCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<int> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
        {
            var lastDraftOrder = await _orderRepository.GetLastOpenOrderAsync(request.CustomerId);
            if(lastDraftOrder == null)
            {
                Order NewOrder = new Order
                {
                    Customer = request.CustomerId,
                    SubTotal = 0,
                    Status = 0
                };
                lastDraftOrder = await _orderRepository.AddOrderAsync(NewOrder);
            }
            var ProductInfo = await _orderRepository.GetProductAsync(request.ProductId);
            if(ProductInfo == null)
                return -1;
            var orderitem = new OrderItem
            {
                OrderId = lastDraftOrder.Id,
                Quantity = request.Quantity,
                Price = ProductInfo.Price,
                ProductId = request.ProductId
            };
            await _orderRepository.AddOrderItemAsync(orderitem);
            var OrderInfo = await _orderRepository.GetOrderItemsAsync(orderitem.OrderId);
            if (OrderInfo.OrderItem == null)
                return -1;
            return OrderInfo.OrderItem.Count();   
        }
    }
}