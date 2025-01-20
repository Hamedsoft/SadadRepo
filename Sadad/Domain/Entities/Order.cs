namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int Customer { get; set; }
        public double SubTotal { get; set; }
        public int Status { get; set; }
        
        public ICollection<OrderItem> OrderItems { get; set; }
    }

}
