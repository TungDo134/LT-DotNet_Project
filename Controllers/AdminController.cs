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


        // lay ra ds sp de hien thi cho admin
        public async Task<IActionResult> listProduct()
        {
            var client = _httpClientFactory.CreateClient();

            // Gọi API để lấy danh sách sản phẩm
            // responseP chứa kết quả phản hồi từ API
            var responseP = await client.GetAsync($"https://localhost:7258/api/ProductAPI");
            IEnumerable<ProductVM> products = null;

            if (responseP.IsSuccessStatusCode)
            {
                var jsonResponseP = await responseP.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(jsonResponseP);
            }

            return View(products);
        }

        // Hiển thị trang thêm sản phẩm và xử lí sk thêm sp
        public async Task<IActionResult> AddProduct()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            // Gọi API để lấy danh sách danh mục
            var responseC = await client.GetAsync($"https://localhost:7258/api/CateAPI");
            IEnumerable<CateVM> categories = null;

            if (responseC.IsSuccessStatusCode)
            {
                var jsonResponseC = await responseC.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<IEnumerable<CateVM>>(jsonResponseC);

            }

            return View(categories);
        }

        // Hiển thị trang chỉnh sửa sản phẩm 
        public async Task<IActionResult> EditProduct(int id)
        {
            
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/DetailAPI/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Khong tim thay san pham" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

            var product = await response.Content.ReadFromJsonAsync<ProductVM>();
            return View(product); // Trả về View với thông tin sản phẩm

           
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
