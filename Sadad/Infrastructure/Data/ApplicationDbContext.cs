using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .HasColumnType("Nvarchar(256)");

            modelBuilder.Entity<Product>()
            .Property(p => p.ImageFileName)
            .HasColumnType("Nvarchar(256)");

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Product)
                .WithMany(o => o.ProdOrderItems)
                .HasForeignKey(o => o.ProductId);

            // Fill Initial Data
            modelBuilder.Entity<Product>().HasData(
                new Product 
                { 
                    Id = 1,
                    Name = "شیائومی مدل Redmi Note 13 Pro",
                    Rate = 4,
                    Price = 275000000,
                    ImageFileName = "1.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "اپل مدل iPhone 13 CH 256",
                    Rate = 3,
                    Price = 575000000,
                    ImageFileName = "2.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "اپل مدل iPhone 13 pro ZAA 128",
                    Rate = 5,
                    Price = 757000000,
                    ImageFileName = "3.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "سامسونگ مدل Galaxy A15 128",
                    Rate = 4,
                    Price = 265000000,
                    ImageFileName = "4.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "سامسونگ مدل Galaxy A35 128",
                    Rate = 3,
                    Price = 219000000,
                    ImageFileName = "5.jpg"
                },
                new Product
                {
                    Id = 6,
                    Name = "سامسونگ مدل Galaxy A05 256",
                    Rate = 1,
                    Price = 175000000,
                    ImageFileName = "6.jpg"
                },
                new Product
                {
                    Id = 7,
                    Name = "نوکیا مدل 1100",
                    Rate = 5,
                    Price = 75000000,
                    ImageFileName = "7.jpg"
                },
                new Product
                {
                    Id = 8,
                    Name = "سامسونگ مدل Galaxy A55 512",
                    Rate = 2,
                    Price = 143000000,
                    ImageFileName = "8.jpg"
                },
                new Product
                {
                    Id = 9,
                    Name = "سامسونگ مدل Galaxy S24 512",
                    Rate = 4,
                    Price = 331000000,
                    ImageFileName = "9.jpg"
                }
            );
        }
    }
}
