using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModels;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WebBanLapTop.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using WebBanLapTop.Helpers;
using Newtonsoft.Json;
using System.Net.Http;

namespace WebBanLapTop.Controllers
{
    public class AccountController : Controller
    {
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(LaptopShopContext context, IHttpClientFactory httpClientFactory)
        {
            db = context;
            _httpClientFactory = httpClientFactory;
        }

        // Hiển thị trang đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        // Hiển thị trang đổi mật khẩu
        [HttpGet]
        [Authorize]
        public IActionResult ChangePass(int id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }

        // thực hiện thay đổi
        [HttpPost]
        [Authorize]
        public IActionResult ChangePass([FromBody] ChangePass model)
        {

            var user = db.Users.FirstOrDefault(p => p.Iddn == model.Id &&
            p.MatkhauDn == MaHoaMK.ToSHA1(model.oldPassword));

            if (user == null)
            {
                
                return BadRequest(ModelState);
            }

            user.MatkhauDn = MaHoaMK.ToSHA1(model.newPassword); // Mã hóa mật khẩu
            db.Users.Update(user);
            db.SaveChanges();


            return RedirectToAction("SignIn", "Account");
        }



        [HttpGet]
        public ContentResult checkUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return Content("Tên người dùng không được để trống.");
            }

            var user = db.Users.FirstOrDefault(u => u.TenDn == userName);
            if (user != null)
            {
                return Content("Tên người dùng đã tồn tại");
            }

            return Content("");
        }

        [HttpGet]
        public ContentResult checkEmail(string email)
        {
            // Kiểm tra email không được để trống
            if (string.IsNullOrEmpty(email))
            {
                return Content("Email không được để trống.");
            }

            // Kiểm tra định dạng email hợp lệ
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            if (!emailRegex.IsMatch(email))
            {
                return Content("Định dạng email không hợp lệ.");
            }

            // Kiểm tra email đã tồn tại trong cơ sở dữ liệu chưa
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return Content("Email đã tồn tại");
            }

            return Content("");
        }

        [HttpGet]
        public ContentResult checkPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return Content("Số điện thoại không được để trống.");
            }

            var phonePattern = new System.Text.RegularExpressions.Regex(@"^0\d{9}$");
            if (!phonePattern.IsMatch(phone))
            {
                return Content("Số điện thoại không hợp lệ (Phải bắt đầu bằng 0 và có 10 chữ số).");
            }

            return Content("");
        }

        [HttpGet]
        public ContentResult checkPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Content("Mật khẩu không được để trống.");
            }

            var passwordPattern = new System.Text.RegularExpressions.Regex(@"^(?=.*[!@#$%^&*])(?=.{6,})");
            if (!passwordPattern.IsMatch(password))
            {
                return Content("Mật khẩu phải có ít nhất 6 ký tự và chứa ký tự đặc biệt.");
            }

            return Content("");
        }

        [HttpPost]
        public IActionResult Register([FromBody] RegisterVM model)
        {
            Console.WriteLine($"TenDn: {model.TenDn}, Email: {model.Email}, Sdt: {model.Sdt}, MatkhauDn: {model.MatkhauDn}");

            var maxId = db.Users.Max(u => u.Iddn);
            int newId = maxId + 1;

            var user = new User
            {
                Iddn = newId,
                TenDn = model.TenDn,
                Email = model.Email,
                Sdt = model.Sdt,
                MatkhauDn = MaHoaMK.ToSHA1(model.MatkhauDn), // Mã hóa mật khẩu
                Quyen = false
            };

            db.Users.Add(user);
            db.SaveChanges();

            return RedirectToAction("SignIn", "Account");
        }


        [HttpGet]
        public IActionResult SignIn(String? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginVM model, String? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                var customers = db.Users.SingleOrDefault(p => p.TenDn == model.Username && p.MatkhauDn == MaHoaMK.ToSHA1(model.Password));
                if (customers == null)
                {
                    ModelState.AddModelError("error", "Sai thông tin đăng nhập");
                }
                else
                {

                    // Tạo claims để xác thực và phân quyền
                    var claims = new List<Claim>

            {
                new Claim(ClaimTypes.NameIdentifier, customers.Iddn.ToString()), // Lưu userId vào claim
                new Claim(ClaimTypes.Name, customers.TenDn ?? ""),
                new Claim(ClaimTypes.Email, customers.Email ?? ""),
                new Claim("MatKhau", customers.MatkhauDn),
                new Claim(ClaimTypes.MobilePhone, customers.Sdt?? ""),
                new Claim (ClaimTypes.StreetAddress,customers.DiaChi?? ""),
                 new Claim(ClaimTypes.Role, customers.Quyen.HasValue && customers.Quyen.Value ? "Admin" : "User"),
            };


                    var claimsIndetity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrinciple = new ClaimsPrincipal(claimsIndetity);

                    await HttpContext.SignInAsync(claimsPrinciple);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        // Về url mà cần đăng nhập trước đó
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        // Về trang chủ
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View();
        }


        [Authorize]
        public async Task<IActionResult> Profile()
        {

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            ViewBag.Email = string.IsNullOrEmpty(email) ? "" : email;

            var address = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.StreetAddress)?.Value;
            ViewBag.Address = string.IsNullOrEmpty(address) ? "" : address;

            var phone = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;
            ViewBag.Phone = string.IsNullOrEmpty(phone) ? "" : phone;

            var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            ViewBag.Id = string.IsNullOrEmpty(id) ? "" : id;

            // lấy ra danh sách lịch sử mua hàng của người dùng
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/OrderAPI/orderUserID/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Không tìm thấy đơn hàng" });
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Sử dụng JsonSerializer để giải mã JSON vào đối tượng OrderDetailVM
            var orders = JsonConvert.DeserializeObject<List<Hoadon>>(jsonResponse);

            return View(orders);

        }

        [Authorize]
        public IActionResult EditProfile()
        {

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            ViewBag.Email = string.IsNullOrEmpty(email) ? "" : email;

            var address = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.StreetAddress)?.Value;
            ViewBag.Address = string.IsNullOrEmpty(address) ? "" : address;

            var phone = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;
            ViewBag.Phone = string.IsNullOrEmpty(phone) ? "" : phone;

            var id = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            ViewBag.Id = string.IsNullOrEmpty(id) ? "" : id;

            return View();

        }


        public async Task<IActionResult> UpdateProfile(int id, String fullname, String email, String phone, String address)
        {


            var user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound(); // Trả về lỗi nếu không tìm thấy User
            }

            // Cập nhật các thuộc tính
            user.TenDn = fullname;
            user.Email = email;
            user.Sdt = phone;
            user.DiaChi = address;

            // Lưu thay đổi
            db.Users.Update(user);
            db.SaveChanges();

            // Tạo claims để xác thực và phân quyền
            var claims = new List<Claim>

            {
                new Claim(ClaimTypes.NameIdentifier, user.Iddn.ToString()), // Lưu userId vào claim
                new Claim(ClaimTypes.Name, user.TenDn ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("MatKhau", user.MatkhauDn),
                new Claim(ClaimTypes.MobilePhone, user.Sdt),
                new Claim (ClaimTypes.StreetAddress,user.DiaChi),
                new Claim(ClaimTypes.Role, (bool)user.Quyen ? "Admin" : "User"),
            };


            var claimsIndetity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrinciple = new ClaimsPrincipal(claimsIndetity);

            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(claimsPrinciple);


            return RedirectToAction("Profile");

        }




        [Authorize]
        public async Task<IActionResult> GetOrderDetail(int id)
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


        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            // về trang chủ
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            // tới trang admin
            return RedirectToAction("Index", "Admin");
        }
    }
}