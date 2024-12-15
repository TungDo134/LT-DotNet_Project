using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using WebBanLapTop.Data;
using WebBanLapTop.Models;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;


        public HomeController(ILogger<HomeController> logger, LaptopShopContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            db = context;
            _httpClientFactory = httpClientFactory;
        }




        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/ProductAPI");

            if (response.IsSuccessStatusCode)
            {
                // Đọc nội dung JSON trả về dưới dạng chuỗi
                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Chuyển đổi chuỗi JSON thành IEnumerable<HomeVM>
                IEnumerable<HomeVM> products = JsonConvert.DeserializeObject<IEnumerable<HomeVM>>(jsonResponse);

                return View(products); // Trả về View với danh sách sản phẩm
            }
            else
            {

                return View("Error", new { Message = "Product not found" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
