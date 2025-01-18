using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Application.DTOs;
using Application.Exceptions;
using Domain.Enums;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        #region Post Repository
        public async Task<Order> AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }
        public async Task AddOrderItemAsync(OrderItem orderitem)
        {
            await _context.OrderItems.AddAsync(orderitem);
            await _context.SaveChangesAsync();
        }
        #endregion
        #region Get Repository
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        public async Task<IEnumerable<ProductDto>> GetAllProductAsync()
        {
            var products = await _context.Products
                .Select(o => new ProductDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Rate = o.Rate,
                    Price = o.Price,
                    ImageFileName = o.ImageFileName
                }).ToListAsync();
            return products;
        }
        public async Task<ProductDto> GetProductAsync(int ProductId)
        {
            var product = await _context.Products
                .Where(p => p.Id == ProductId)
                .Select(o => new ProductDto
                {
                    Id = o.Id,
                    Name = o.Name,
                    Rate = o.Rate,
                    Price = o.Price,
                    ImageFileName = o.ImageFileName
                }).FirstOrDefaultAsync();
            return product;
        }
        public async Task<OrderListDto> GetOrderItemsAsync(int OrderId)
        {
            OrderListDto result = new OrderListDto();
            var order = await _context.Orders
            .Where(o => o.Id == OrderId)
            .Select(o => new OrderListDto
            {
                Order = new OrderDto
                {
                    Customer = o.Customer,
                    SubTotal = o.SubTotal,
                    Status = o.Status
                },
                OrderItem = o.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    Price = oi.Price,
                    Quantity = oi.Quantity,
                    ProductId = oi.Product.Id,
                    ProductName = oi.Product.Name
                }).ToList()
            })
            .FirstOrDefaultAsync();
            return order == null ? result : order;
        }
        public async Task<List<OrderListGroupDto>> GetOrderItemsGroupAsync(int Customerid)
        {
            List<OrderItemDto> DraftOrderItems = await GetLastOpenOrderItemsAsync(Customerid);
            List<OrderListGroupDto> Model = DraftOrderItems
                .GroupBy(M => new { M.OrderId, M.ProductId, M.ProductName, M.Price })
                .Select(B => new OrderListGroupDto
                {
                    OrderId = B.First().OrderId,
                    ProductId = B.First().ProductId,
                    ProductName = B.First().ProductName,
                    Price = B.First().Price,
                    Quantity = B.Sum(x => x.Quantity),
                    Total = B.Sum(x => x.Quantity) * B.Sum(x => x.Price)
                }).ToList();
            return Model;
        }
        public async Task<List<OrderItemDto>> GetLastOpenOrderItemsAsync(int CustomerId)
        {
            List<OrderItemDto> result = new List<OrderItemDto>();
            var order = await _context.OrderItems
                              .Where(O => O.Order.Status == 0 && O.Order.Customer == CustomerId)
                              .Select(oi => new OrderItemDto
                              {
                                  Id = oi.Id,
                                  OrderId = oi.OrderId,
                                  Quantity = oi.Quantity,
                                  Price = oi.Price,
                                  ProductId = oi.ProductId,
                                  ProductName = oi.Product.Name
                              }).ToListAsync();
            return order == null ? result : order;
        }
        public async Task<Order> GetLastOpenOrderAsync(int CustomerId)
        {
            return await _context.Orders
                              .Where(O => O.Status == 0 && O.Customer == CustomerId)
                              .OrderBy(O => O.Id)
                              .LastOrDefaultAsync();
        }
        #endregion
        #region Delete Repository
        public async Task<OrderItem> GetOrderItem(int OrderId, int ProductId)
        {
            return _context.OrderItems.Where(M => M.OrderId == OrderId && M.ProductId == ProductId).OrderBy(M => M.Id).LastOrDefault();
        }
        public async Task DeleteOrderItems(int OrderId, int ProductId)
        {
            OrderItem Item = await GetOrderItem(OrderId, ProductId);
            _context.OrderItems.Remove(Item);

            Order order = _context.Orders.Where(R => R.Id == Item.OrderId).FirstOrDefault();
            if(order == null)
                throw new CustomException(ErrorCode.InvalidOrderId);

            List<OrderItemDto> OrderItems = await GetLastOpenOrderItemsAsync(order.Customer);
            if (OrderItems.Count() == 0)
                _context.Orders.Remove(order);
            else
                order.SubTotal = OrderItems.Sum(M => M.Quantity * M.Price);
            _ = _context.SaveChangesAsync();
        }
        #endregion
        #region Update Repository
        public async Task CommitOrder(int OrderId)
        {
            Order Item = _context.Orders.Where(M => M.Id == OrderId).FirstOrDefault();
            if (Item == null)
                throw new CustomException(ErrorCode.ModelIsNull);

            Item.Status = 1;
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
