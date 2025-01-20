namespace Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        // Foreign Keys
        public int OrderId { get; set; }  // Foreign key to Order
        public int ProductId { get; set; }  // Foreign key to Product

        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
