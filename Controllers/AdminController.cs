using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // Hiển thị trang thêm sản phẩm và xử lí sk thêm sp
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
            var response = await client.GetAsync($"https://localhost:7258/api/DetailAPI/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Khong tim thay san pham" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

            var product = await response.Content.ReadFromJsonAsync<ProductVM>();
            return View(product); // Trả về View với thông tin sản phẩm


        }



        // ds the loai
        public IActionResult ListCategory()
        {
            var categories = db.Danhmucsanphams
                .Select(c => new CateVM
                {
                    Id = c.MaDanhMuc,
                    Name = c.TenDanhMuc,
                    Hinh = c.HinhDanhMuc
                })
                .ToList();

            return View(categories); // Trả về view ListCategory với danh sách danh mục
        }



        // them the loai
        public IActionResult ShowAddCate()
        {

            return View();

        }
        public IActionResult AddCate(String name, String cateImg, String cateID)
        {
            // Lấy giá trị từ form
            String _name = name;
            String _hinh = cateImg;
            String _id = cateID;
            int CateId = 0;

            // Kiểm tra nếu cateID không rỗng thì parse sang int
            if (_id != null)
            {
                CateId = Int32.Parse(_id);
            }

            // Kiểm tra nếu danh mục đã tồn tại trong cơ sở dữ liệu (theo tên hoặc ID)
            var existingCategory = db.Danhmucsanphams.FirstOrDefault(c => c.TenDanhMuc == _name || c.MaDanhMuc == CateId);

            if (existingCategory != null)
            {
                // Nếu danh mục đã tồn tại, trả về thông báo lỗi
                ViewBag.ErrorMessage = "Danh mục này đã tồn tại!";
                return View();  // Trả về lại view AddCate và hiển thị thông báo lỗi
            }

            // Nếu danh mục chưa tồn tại, tiến hành thêm mới
            Danhmucsanpham data = new Danhmucsanpham
            {
                MaDanhMuc = CateId,
                TenDanhMuc = _name,
                HinhDanhMuc = _hinh ?? ""
            };

            db.Danhmucsanphams.Add(data);
            db.SaveChanges();

            // Sau khi thêm xong, chuyển về danh sách danh mục
            return RedirectToAction("ListCategory");
        }



        //xoa
        public IActionResult DeleteCate(int id)
        {
            Danhmucsanpham data = db.Danhmucsanphams.Find(id);

            db.Danhmucsanphams.Remove(data);
            db.SaveChanges();


            return RedirectToAction("ListCategory");
        }

        //hien thi trang edit cate
        public IActionResult ShowEditCate(int id)
        {
            var category = db.Danhmucsanphams
                .Where(c => c.MaDanhMuc == id)
                .Select(c => new CateVM
                {
                    Id = c.MaDanhMuc,
                    Name = c.TenDanhMuc ?? "",
                    Hinh = c.HinhDanhMuc ?? "" // Chuyển c.Image thành Hinh trong ViewModel
                })
                .FirstOrDefault(); // Lấy một đối tượng đơn lẻ

            if (category == null)
            {
                return NotFound("Danh mục không tồn tại");
            }

            return View(category); // Truyền đối tượng category vào view
        }


        // thuc hien viejc update cate
        //public IActionResult EditCate(int id)
        //{
        //    // Tìm danh mục cần chỉnh sửa theo ID
        //    var category = db.Danhmucsanphams
        //        .Where(c => c.MaDanhMuc == id)
        //        .Select(c => new CateVM
        //        {
        //            Id = c.MaDanhMuc,
        //            Name = c.TenDanhMuc ?? "",
        //            Hinh = c.HinhDanhMuc ?? ""
        //        })
        //        .FirstOrDefault();

        //    if (category == null)
        //    {
        //        return NotFound("Danh mục không tồn tại");
        //    }

        //    // Truyền thông tin danh mục sang View
        //    return View(category);
        //}


        public IActionResult SaveEditCate(int id, String name, String cateImg)
        {


            // Tìm danh mục cần chỉnh sửa
            var category = db.Danhmucsanphams.Find(id);
            if (category == null)
            {
                return NotFound("Danh mục không tồn tại");
            }


            category.TenDanhMuc = name;

            category.HinhDanhMuc = cateImg;

            Console.WriteLine();

            db.Danhmucsanphams.Update(category);
            db.SaveChanges();

            return RedirectToAction("ListCategory");
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
            if (ModelState.IsValid)
            {
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
                    TenDn = newUser.Name,
                    Email = newUser.email,
                    DiaChi = newUser.Address,
                    Sdt = newUser.PhoneNumber,
                    MatkhauDn = newUser.Pass,
                    Quyen = newUser.Role // true nếu vai trò là Admin
                };

                db.Users.Add(user);
                db.SaveChanges();

                // Chuyển hướng về danh sách người dùng
                return RedirectToAction("listUser");
            }

            // Dữ liệu không hợp lệ, trả lại form
            return View("ShowAddUser", newUser);
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

    internal class DanhMucSanPham
    {
        internal int MaDanhMuc;
        internal string TenDanhMuc;
        internal string HinhDanhMuc;
    }
}

