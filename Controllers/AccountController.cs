
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

namespace WebBanLapTop.Controllers
{
    public class AccountController : Controller
    {
        private readonly LaptopShopContext db;

        public AccountController(LaptopShopContext context)
        {
            db = context;
        }

        // Hiển thị trang đăng ký
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        // Xử lý đăng ký
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu email hoặc tên đăng nhập đã tồn tại (gộp truy vấn)
                var existingUser = db.Users.FirstOrDefault(u => u.Email == model.Email || u.TenDn == model.TenDn);

                if (existingUser != null)
                {
                    if (existingUser.Email == model.Email)
                        ModelState.AddModelError("Email", "This email is already registered.");
                    if (existingUser.TenDn == model.TenDn)
                        ModelState.AddModelError("TenDn", "This username is already taken.");
                }

                // Xác nhận lại ModelState
                if (ModelState.IsValid)
                {
                    // Lưu thông tin người dùng vào cơ sở dữ liệu
                    var user = new User
                    {
                        TenDn = model.TenDn,
                        Email = model.Email,
                        Sdt = model.Sdt,
                        MatkhauDn = MaHoaMK.ToSHA1(model.MatkhauDn) // Mã hóa mật khẩu
                    };

                    db.Users.Add(user);
                    db.SaveChanges();

                    // Điều hướng về trang đăng nhập
                    return RedirectToAction("SignIn", "Account");
                }
            }

            // Trả về view với thông báo lỗi
            return View(model);
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
                new Claim(ClaimTypes.MobilePhone, customers.Sdt),
                new Claim (ClaimTypes.StreetAddress,customers.DiaChi),
                new Claim(ClaimTypes.Role, (bool)customers.Quyen ? "Admin" : "User"),
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
        public IActionResult Profile()
        {

            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            ViewBag.Email = string.IsNullOrEmpty(email) ? "" : email;

            var address = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.StreetAddress)?.Value;
            ViewBag.Address = string.IsNullOrEmpty(address) ? "" : address;

            var phone = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value;
            ViewBag.Phone = string.IsNullOrEmpty(phone) ? "" : phone;

            return View();
        }


        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            // về trang chủ
            return RedirectToAction("Index", "Home");
        }


        //[Authorize(Roles = "Admin")]
        public IActionResult AdminPage()
        {
            // tới trang admin
            return RedirectToAction("Index", "Admin");
        }
    }
}
