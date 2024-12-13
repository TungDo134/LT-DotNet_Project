using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class DetailAPIController : ControllerBase
    {
        private readonly LaptopShopContext db;
        public DetailAPIController(LaptopShopContext context)
        {
            db = context;
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            // Lấy sản phẩm dựa trên ID
            var product = db.ChiTietSanPhams
                .Where(p => p.MaSp == id)
                .Select(p => new HomeVM
                {
                    Id = p.MaSp,
                    Name = p.TenSp ?? "",
                    Img = p.HinhAnh ?? "",
                    Price = p.DonGia ?? 0,
                    Description = p.ThongTinSp ?? "",
                    KhoiLuong = p.KhoiLuong ?? 0
                })
                .FirstOrDefault();

            // Kiểm tra nếu không tìm thấy sản phẩm
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Trả về sản phẩm nếu tìm thấy
            return Ok(product);
        }

    }
}
