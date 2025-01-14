using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{

    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(ILogger<AdminController> logger, LaptopShopContext context, IHttpClientFactory httpClientFactory)
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

        // Hiển thị trang thêm sản phẩm
        public async Task<IActionResult> AddProduct()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            // Gọi API để lấy danh sách danh mục (để chọn danh mục cho sản phẩm)
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

            // Gọi API để lấy danh sách sản phẩm
            var responseP = await client.GetAsync($"https://localhost:7258/api/DetailAPI/{id}");
            ProductVM products = null;

            if (responseP.IsSuccessStatusCode)
            {
                var jsonResponseP = await responseP.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<ProductVM>(jsonResponseP);
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
            var viewModel = new ProductAdminVM
            {
                Product = products ?? new ProductVM(), // Nếu API lỗi, gán danh sách trống
                Categories = categories ?? new List<CateVM>()
            };

            ViewBag.SelectedCategory = products.MaDanhMuc;

            return View(viewModel);

        }



        // ds the loai + them the loai 
        public async Task<IActionResult> ListCategory()
        {
            var client = _httpClientFactory.CreateClient();
            // Gọi API để lấy danh sách danh mục
            var responseC = await client.GetAsync($"https://localhost:7258/api/CateAPI");
            IEnumerable<CateVM> categories = null;

            if (responseC.IsSuccessStatusCode)
            {
                var jsonResponseC = await responseC.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<IEnumerable<CateVM>>(jsonResponseC);
            }

           
            return View(categories); // Trả về view ListCategory với danh sách danh mục
        }


        //hien thi trang edit cate
        public async Task<IActionResult> ShowEditCate(int id)
        {
            var client = _httpClientFactory.CreateClient();
            // Gọi API để lấy danh sách danh mục
            var responseC = await client.GetAsync($"https://localhost:7258/api/CateAPI/getById/{id}");
            Danhmucsanpham category = null;


            if (responseC.IsSuccessStatusCode)
            {
                var jsonResponseP = await responseC.Content.ReadAsStringAsync();
                category = await responseC.Content.ReadFromJsonAsync<Danhmucsanpham>();
            }

            
            return View(category); // Truyền đối tượng category vào view
        }


       

        // Lấy danh sách đơn hàng
        public async Task<IActionResult> ListOrder()
        {
            var client = _httpClientFactory.CreateClient();

            // Gọi API để lấy danh sách đơn hàng
            var response = await client.GetAsync("https://localhost:7258/api/OrderAPI");
            IEnumerable<Hoadon> orders = null;

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<IEnumerable<Hoadon>>(jsonResponse);
            }
            else
            {
                ViewBag.ErrorMessage = $"Không thể tải danh sách đơn hàng. Mã lỗi: {response.StatusCode}";
                orders = new List<Hoadon>(); // Trả về danh sách rỗng nếu không thành công
            }

            return View(orders);
        }

        //chi tiet hoa don
        public async Task<IActionResult> OrderDetail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/OrderAPI/detail/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Không tìm thấy đơn hàng" });
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Sử dụng JsonSerializer để giải mã JSON vào đối tượng OrderDetailVM
            var orderDetail = JsonConvert.DeserializeObject<OrderDetailVM>(jsonResponse);

            return View(orderDetail);
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

    internal class DanhMucSanPham
    {
        internal int MaDanhMuc;
        internal string TenDanhMuc;
        internal string HinhDanhMuc;
    }
}
