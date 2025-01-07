using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{

    public class AdminController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(ILogger<HomeController> logger, LaptopShopContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            db = context;
            _httpClientFactory = httpClientFactory;
        }




        public IActionResult Index()
        {
            return View();
        }


        // ds sp
        public async Task<IActionResult> listProduct()
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

            return View(products);
        }

        // them sp
        public IActionResult addProduct()
        {
            return View();
        }


        // ds the loai
        public IActionResult listCategory()
        {
            return View();
        }

        // them the loai
        public IActionResult addCategory()
        {
            return View();
        }

        // ds don hang
        public IActionResult listOrder()
        {
            return View();
        }

        // ctdh
        public IActionResult orderDetail()
        {
            return View();
        }

        // ds ng dung
        public IActionResult listUser()
        {
            return View();
        }

        // them ng dung
        public IActionResult addUser()
        {
            return View();
        }
    }
}
