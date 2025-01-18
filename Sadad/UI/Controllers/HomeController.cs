using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using UI.Models;
using UI.Services;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderService _orderService;
        private readonly INotyfService _notyfService;

        public HomeController(ILogger<HomeController> logger, IOrderService orderService, INotyfService notyfService)
        {
            _logger = logger;
            _orderService = orderService;
            _notyfService = notyfService;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<ActionResult> Products()
        {
            var products = await _orderService.GetProducts();
            if (products == null || !products.Any())
            {
                ViewData["ErrorDesc"] = "محصولی یافت نشد! لطفا ارتباط خود را با پایگاه داده چک کنید.";
                return View("Error");
            }

            var draftOrderItems = await _orderService.GetLastOpenOrderItems(1);
            ViewData["OpenOrderCount"] = draftOrderItems.Count();
            return View(products);
        }
        //private async Task<List<OrderItemDto>> GetLastOpenOrderItems(int CustomerId = 1)
        //{
        //    List<OrderItemDto> Result = new List<OrderItemDto>();
        //    var response = await _httpClient.GetAsync($"Orders/GetLastOpenOrderItems/{CustomerId}");
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return Result;
        //    }
        //    var jsonResponse = await response.Content.ReadAsStringAsync();
        //    Result = JsonConvert.DeserializeObject<List<OrderItemDto>>(jsonResponse);
        //    return Result;
        //}
        public async Task<IActionResult> Orders(int customerId = 1)
        {
            var orders = await _orderService.GetOrderGroupItems(customerId);
            if (!orders.Any())
            {
                _notyfService.Error("سبد محصولات خالی است");
                return RedirectToAction("Products");
            }

            ViewData["OrderCount"] = orders.Count;
            ViewData["OrderSubTotal"] = orders.Sum(m => m.Total);
            return View(orders);
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CommitOrderAsync(int orderId)
        {
            var isSuccess = await _orderService.CommitOrderAsync(orderId);
            if (!isSuccess)
            {
                _notyfService.Error("خطا در ارسال درخواست");
                return View();
            }

            _notyfService.Success("فرایند درخواست سفارش تکمیل شد");
            ViewData["OrderId"] = orderId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddOrderItem(int customerId, int productId)
        {
            var isSuccess = await _orderService.AddOrderItem(customerId, productId);
            if (!isSuccess)
            {
                _notyfService.Error("خطا در ارسال سفارش");
            }
            else
            {
                _notyfService.Success("محصول به سفارشات شما اضافه شد");
            }
            var orders = await _orderService.GetLastOpenOrderItems(customerId);
            return Content(orders.Count().ToString());
        }
        [HttpGet]
        public async Task<IActionResult> RemoveOrderItem(int orderId, int productId)
        {
            var isSuccess = await _orderService.RemoveOrderItem(orderId, productId);
            if (!isSuccess)
            {
                _notyfService.Error("خطا در ارسال درخواست");
            }
            else
            {
                _notyfService.Success("محصول از لیست سفارشات حذف شد");
            }

            return RedirectToAction("Orders", new { customerId = 1 });
        }
    }
}