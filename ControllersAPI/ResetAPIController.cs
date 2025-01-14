using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanLapTop.Data;
using WebBanLapTop.Helpers;

namespace WebBanLapTop.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResetAPIController : ControllerBase
    {
        private readonly LaptopShopContext db;
        public ResetAPIController(LaptopShopContext context)
        {
            db = context;
        }

        // API reset mật khẩu
        [HttpPost("reset-password/{userId}")]
        public IActionResult ResetPassword(int userId)
        {
            // 1. Kiểm tra quyền admin
            if (!IsAdmin())
            {
                return Unauthorized("Bạn không có quyền thực hiện hành động này.");
            }


            // 3. Tìm người dùng theo userId
            var user = db.Users.FirstOrDefault(u => u.Iddn == userId);
            if (user == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // 4. Gán mật khẩu mới bằng 1
            user.MatkhauDn = MaHoaMK.ToSHA1("1");

            // 5. Lưu thay đổi vào cơ sở dữ liệu
            db.Users.Update(user);
            db.SaveChanges();

            // 6. Phản hồi kết quả
            return Ok(new
            {
                Message = $"Mật khẩu của người dùng {user.TenDn} đã được reset.",
                NewPassword = user.MatkhauDn
            });
        }

       

        // Phương thức kiểm tra quyền admin
        private bool IsAdmin()
        {
            // Logic kiểm tra quyền admin, ví dụ dựa trên JWT hoặc Claim
            return User.IsInRole("Admin"); // Kiểm tra vai trò "Admin" trong token
        }
    }
}
