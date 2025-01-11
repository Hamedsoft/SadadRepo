using Newtonsoft.Json;
using Shared.DTOs;
using System.Text;

namespace UI.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var response = await _httpClient.GetAsync("Orders/GetProducts");
            if (!response.IsSuccessStatusCode) return new List<ProductDto>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ProductDto>>(jsonResponse);
        }

        public async Task<List<OrderItemDto>> GetLastOpenOrderItems(int customerId)
        {
            var response = await _httpClient.GetAsync($"Orders/GetLastOpenOrderItems/{customerId}");
            if (!response.IsSuccessStatusCode) return new List<OrderItemDto>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrderItemDto>>(jsonResponse);
        }

        public async Task<List<OrderListGroupDto>> GetOrderGroupItems(int customerId)
        {
            var response = await _httpClient.GetAsync($"Orders/GetOrderGroupItems/{customerId}");
            if (!response.IsSuccessStatusCode) return new List<OrderListGroupDto>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<OrderListGroupDto>>(jsonResponse);
        }

        public async Task<bool> CommitOrderAsync(int orderId)
        {
            var orderData = new { orderId };
            var jsonData = JsonConvert.SerializeObject(orderData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("Orders/CommitOrder", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddOrderItem(int customerId, int productId)
        {
            var orderItemData = new { customerId, productId };
            var jsonData = JsonConvert.SerializeObject(orderItemData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Orders/AddOrderItem", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveOrderItem(int orderId, int productId)
        {
            var orderItemData = new { orderId, productId };
            var jsonData = JsonConvert.SerializeObject(orderItemData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "Orders/RemoveOrderItems") { Content = content });
            return response.IsSuccessStatusCode;
        }
    }
}
