namespace Shared.DTOs
{
    public class OrderDto
    {
        public int Customer { get; set; }
        public double SubTotal { get; set; }
        public int Status { get; set; }
    }
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
    public class OrderListGroupDto
    {
        public int OrderId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Total { get; set; }
    }
    public class AddOrderItemDto
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
    }
    public class DeleteOrderItemDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
    public class CommitOrderDto
    {
        public int OrderId { get; set; }
    }
    public class OrderListDto
    {
        public OrderDto Order { get; set; }
        public List<OrderItemDto> OrderItem { get; set; }
    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
        public double Price { get; set; }
        public string ? ImageFileName { get; set; }
    }
}
