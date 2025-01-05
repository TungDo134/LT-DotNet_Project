using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly LaptopShopContext db;
        public ProductAPIController(LaptopShopContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
           
            var products = db.ChiTietSanPhams
                .Select(p => new ProductVM
                {
                    Id = p.MaSp,
                    Name = p.TenSp ?? "",
                    Img = p.HinhAnh ?? "",
                    Price = p.DonGia ?? 0,
                    Description = p.ThongTinSp ?? "",
                    KhoiLuong = p.KhoiLuong ?? 0
                });


            // Kiểm tra nếu không tìm thấy sản phẩm
            if (products == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Trả về sản phẩm nếu tìm thấy
            return Ok(products);
        }
    }
}
