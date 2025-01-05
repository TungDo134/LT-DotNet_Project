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

            // Gọi API để lấy danh sách sản phẩm
            var responseP = await client.GetAsync($"https://localhost:7258/api/ProductAPI");
            IEnumerable<ProductVM> products = null;

            if (responseP.IsSuccessStatusCode)
            {
                var jsonResponseP = await responseP.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(jsonResponseP);
            }

            // Gọi API để lấy danh sách danh mục
            var responseC = await client.GetAsync($"https://localhost:7258/api/CateAPI");
            IEnumerable<CateVM> categories = null;

            if (responseC.IsSuccessStatusCode)
            {
                var jsonResponseC = await responseC.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<IEnumerable<CateVM>>(jsonResponseC);
            }

            // Tạo ViewModel và truyền dữ liệu vào View
            var viewModel = new HomeVM
            {
                Products = products ?? new List<ProductVM>(), // Nếu API lỗi, gán danh sách trống
                Categories = categories ?? new List<CateVM>()
            };

            return View(viewModel);
        }



        public async Task<IActionResult> Cate()
        {
            var client = _httpClientFactory.CreateClient();
            var responseC = await client.GetAsync($"https://localhost:7258/api/ProductAPI");


            if (responseC.IsSuccessStatusCode)
            {
                // Đọc nội dung JSON trả về dưới dạng chuỗi
                var jsonResponse = await responseC.Content.ReadAsStringAsync();

                // Chuyển đổi chuỗi JSON thành IEnumerable<HomeVM>
                IEnumerable<CateVM> cates = JsonConvert.DeserializeObject<IEnumerable<CateVM>>(jsonResponse);

                return View(cates); // Trả về View với danh sách sản phẩm
            }
            else
            {

                return View("Error", new { Message = "Categrogy not found" }); // Xử lý lỗi nếu không tìm thấy danh mục
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
