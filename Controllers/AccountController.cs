using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModels;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
                        MatkhauDn = model.MatkhauDn // Mã hóa mật khẩu
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
        public IActionResult SignIn()
        {
            return View();
        }

    }
}
