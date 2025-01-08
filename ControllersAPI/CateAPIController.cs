using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class CateAPIController : ControllerBase
    {
        private readonly LaptopShopContext db;
        public CateAPIController(LaptopShopContext context)
        {
            db = context;
        }


        public IActionResult Index()
        {
            // lay ra danh muc 
            var cates = db.Danhmucsanphams.Select(c => new CateVM
            {
                Id = c.MaDanhMuc,
                Name = c.TenDanhMuc ?? "",
                Hinh = c.HinhDanhMuc ?? ""

            });
            // Kiểm tra nếu không tìm thấy sản phẩm
            if (cates == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Trả về sản phẩm nếu tìm thấy
            return Ok(cates);
        }
    }
}
