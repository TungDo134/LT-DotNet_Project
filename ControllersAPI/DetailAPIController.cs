using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.ControllersAPI
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
                .Select(p => new ProductVM
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

        // lay ds sp dua tren the loai
        [Route("pByCid/{cid}")]
        [HttpGet]
        public IActionResult GetProductByCateIDex(int cid)
        {
            // Lấy sản phẩm dựa trên ID
            var products = db.ChiTietSanPhams
                .Where(p => p.MaDanhMuc == cid)
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
