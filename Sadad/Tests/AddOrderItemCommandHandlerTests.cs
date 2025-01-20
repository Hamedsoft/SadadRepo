using Application.Commands.Orders.AddOrderItem;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class AddOrderItemCommandHandlerTests
    {
        // تست زمانی که سفارش قبلی وجود ندارد
        [Fact]
        public async Task Handle_WhenNoOpenOrderExists_ShouldCreateNewOrderAndReturnItemCount()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();

            // شبیه‌سازی اینکه هیچ سفارشی برای کاربر موجود نیست
            mockOrderRepository
                .Setup(repo => repo.GetLastOpenOrderAsync(It.IsAny<int>()))
                .ReturnsAsync((Order)null);

            // شبیه‌سازی اضافه شدن یک سفارش جدید
            mockOrderRepository
                .Setup(repo => repo.AddOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(new Order { Id = 1 });

            // شبیه‌سازی واکشی اطلاعات محصول
            mockOrderRepository
                .Setup(repo => repo.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductDto { Id = 1, Price = 100 });

            // شبیه‌سازی اضافه شدن آیتم سفارش
            mockOrderRepository
                .Setup(repo => repo.AddOrderItemAsync(It.IsAny<OrderItem>()))
                .Returns(Task.CompletedTask);

            // شبیه‌سازی بازگرداندن اطلاعات سفارش همراه با یک آیتم
            mockOrderRepository
                .Setup(repo => repo.GetOrderItemsAsync(It.IsAny<int>()))
                .ReturnsAsync(new OrderListDto
                {
                    Order = new OrderDto { Customer = 1, SubTotal = 100, Status = 0 },
                    OrderItem = new List<OrderItemDto>
                    {
                        new OrderItemDto { Id = 1, OrderId = 1, Price = 100, Quantity = 2, ProductId = 1, ProductName = "کالای تست" }
                    }
                });

            var handler = new AddOrderItemCommandHandler(mockOrderRepository.Object);
            var command = new AddOrderItemCommand(1, 1, 2);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result); // انتظار داریم تعداد آیتم‌های سفارش 1 باشد
        }

        // تست زمانی که سفارش قبلی وجود دارد
        [Fact]
        public async Task Handle_WhenOpenOrderExists_ShouldAddOrderItemAndReturnItemCount()
        {
            // Arrange
            var mockOrderRepository = new Mock<IOrderRepository>();
            var existingOrder = new Order { Id = 1, Customer = 1, SubTotal = 100, Status = 0 };

            mockOrderRepository
                .Setup(repo => repo.GetLastOpenOrderAsync(It.IsAny<int>()))
                .ReturnsAsync(existingOrder);

            mockOrderRepository
                .Setup(repo => repo.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductDto { Id = 1, Price = 50 });

            mockOrderRepository
                .Setup(repo => repo.AddOrderItemAsync(It.IsAny<OrderItem>()))
                .Returns(Task.CompletedTask);

            mockOrderRepository
                .Setup(repo => repo.GetOrderItemsAsync(It.IsAny<int>()))
                .ReturnsAsync(new OrderListDto
                {
                    Order = new OrderDto { Customer = 1, SubTotal = 100, Status = 0 },
                    OrderItem = new List<OrderItemDto>
                    {
                        new OrderItemDto { Id = 1, OrderId = 1, Price = 50, Quantity = 1, ProductId = 1, ProductName = "کالای شماره 1" }
                    }
                });

            var handler = new AddOrderItemCommandHandler(mockOrderRepository.Object);
            var command = new AddOrderItemCommand(1, 2, 2);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);  // چون یک مورد اضافه شده است
        }
    }
}
