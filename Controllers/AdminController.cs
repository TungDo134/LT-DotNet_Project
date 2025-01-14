using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBanLapTop.Data;
using WebBanLapTop.Helpers;
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
            // Lấy danh sách người dùng từ cơ sở dữ liệu
            var users = db.Users.Select(u => new UserViewModel
            {
                Id = u.Iddn,
                Name = u.TenDn,
                email = u.Email,
                Address = u.DiaChi,
                PhoneNumber = u.Sdt,
                Pass = u.MatkhauDn,
                Role = u.Quyen
            }).ToList();

            return View(users);
        }

        // them ng dung
        public IActionResult ShowAddUser()
        {
            return View(); // Hiển thị form thêm người dùng
        }

        public IActionResult AddUser(UserViewModel newUser)
        {
            var maxId = db.Users.Max(u => u.Iddn);
            int newId = maxId + 1;

            // Kiểm tra email đã tồn tại
            var existingUser = db.Users.FirstOrDefault(u => u.Email == newUser.email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Email đã tồn tại!";
                return View("ShowAddUser", newUser); // Trả lại form với thông báo lỗi
            }

            // Tạo người dùng mới
            var user = new User
            {
                Iddn = newId,
                TenDn = newUser.Name,
                Email = newUser.email,
                DiaChi = newUser.Address ?? "",
                Sdt = newUser.PhoneNumber ?? "",
                MatkhauDn = MaHoaMK.ToSHA1(newUser.Pass),
                Quyen = newUser.Role ?? false // true nếu vai trò là Admin
            };

            db.Users.Add(user);
            db.SaveChanges();

            // Chuyển hướng về danh sách người dùng
            return RedirectToAction("listUser");

        }
        public IActionResult ShowEditUser(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.Iddn == id);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Iddn,
                Name = user.TenDn,
                email = user.Email,
                Address = user.DiaChi,
                PhoneNumber = user.Sdt,
                Pass = user.MatkhauDn,
                Role = user.Quyen
            };

            return View(userViewModel); // Đảm bảo userViewModel không phải là null
        }



        public IActionResult EditUser(UserViewModel updatedUser)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Iddn == updatedUser.Id);
                if (user == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }

                // Chuyển đổi giá trị từ chuỗi thành bool
                user.Quyen = updatedUser.Role; // Giá trị Role trong updatedUser là bool (true hoặc false)

                // Cập nhật thông tin khác
                user.TenDn = updatedUser.Name;
                user.Email = updatedUser.email;
                user.DiaChi = updatedUser.Address;
                user.Sdt = updatedUser.PhoneNumber;
                user.MatkhauDn = updatedUser.Pass;

                db.Users.Update(user);
                db.SaveChanges();

                return RedirectToAction("listUser");
            }

            return View("ShowEditUser", updatedUser);
        }


        // Xử lý xóa người dùng khi đã xác nhận

        public IActionResult DeleteUser(int id)
        {
            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var user = db.Users.FirstOrDefault(u => u.Iddn == id);

            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Xóa người dùng khỏi cơ sở dữ liệu
            db.Users.Remove(user);
            db.SaveChanges();

            // Chuyển hướng đến trang danh sách người dùng sau khi xóa thành công
            return RedirectToAction("ListUser", "Admin");
        }


    }


}

