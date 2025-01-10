using AspNetCoreHero.ToastNotification.Abstractions;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        INotyfService _NotyfService;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient, INotyfService notyfService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _NotyfService = notyfService;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<ActionResult> Products()
        {
            var response = await _httpClient.GetAsync($"Orders/GetProducts");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ProductDto>>(jsonResponse);
            List<OrderItemDto> DraftOrderItems = await GetLastOpenOrderItems();
            ViewData["OpenOrderCount"] = DraftOrderItems.Count();
            return View(data);
        }
        private async Task<List<OrderItemDto>> GetLastOpenOrderItems(int CustomerId = 1)
        {
            List<OrderItemDto> Result = new List<OrderItemDto>();
            var response = await _httpClient.GetAsync($"Orders/GetLastOpenOrderItems/{CustomerId}");
            if (!response.IsSuccessStatusCode)
            {
                return Result;
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Result = JsonConvert.DeserializeObject<List<OrderItemDto>>(jsonResponse);
            return Result;
        }
        public async Task<IActionResult> Orders(int CustomerId = 1)
        {
            var response = await _httpClient.GetAsync($"Orders/GetOrderGroupItems/{CustomerId}");
            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var Model = JsonConvert.DeserializeObject<List<OrderListGroupDto>>(jsonResponse);

            if (Model != null && Model.Count() > 0)
            {
                ViewData["OrderCount"] = Model.Count();
                ViewData["OrderSubTotal"] = Model.Sum(M => M.Total);
                return View(Model);
            }
            else
            {
                _NotyfService.Error("سبد محصولات خالی است");
                return Redirect("Products");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CommitOrderAsync(int OrderId)
        {
            var CommOrderDto = new
            {
                orderId = OrderId
            };

            var jsonData = JsonConvert.SerializeObject(CommOrderDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, "Orders/CommitOrder")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);


            if (!response.IsSuccessStatusCode)
            {
                _NotyfService.Error("خطا در ارسال درخواست");
            }

            var result = await response.Content.ReadAsStringAsync();
            _NotyfService.Success("فرایند درخواست سفارش تکمیل شد");
            ViewData["OrderId"] = OrderId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrderItem(int CustomerId, int ProductId)
        {
            var orderItemData = new
            {
                customerId = CustomerId,
                productId = ProductId
            };

            var jsonData = JsonConvert.SerializeObject(orderItemData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Orders/AddOrderItem", content);

            if (!response.IsSuccessStatusCode)
            {
                _NotyfService.Error("خطا در ارسال سفارش");
            }

            var result = await response.Content.ReadAsStringAsync();
            _NotyfService.Success("محصول به سفارشات شما اضافه شد");
            return Content(result);
        }
        [HttpGet]
        public async Task<IActionResult> RemoveOrderItem(int OrderId, int ProductId)
        {
            var orderItemData = new
            {
                orderId = OrderId,
                productId = ProductId
            };

            var jsonData = JsonConvert.SerializeObject(orderItemData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Delete, "Orders/RemoveOrderItems")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);


            if (!response.IsSuccessStatusCode)
            {
                _NotyfService.Error("خطا در ارسال درخواست");
            }

            var result = await response.Content.ReadAsStringAsync();
            _NotyfService.Success("محصول از لیست سفارشات حذف شد");
            return RedirectToAction("Orders", new { CustomerId = 1 });
        }
    }
}