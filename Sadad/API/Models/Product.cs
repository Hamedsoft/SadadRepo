namespace API.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rate { get; set; }
        public double Price { get; set; }
        public string ImageFileName { get; set; }

        public ICollection<OrderItem> ProdOrderItems { get; set; }
    }
}
