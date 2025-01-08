using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // xóa sản phẩm
        [HttpDelete]
        [Route("delById/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            // Logic để xóa sản phẩm theo ID

            var product = db.ChiTietSanPhams.FirstOrDefault(p => p.MaSp == id);

            if (product == null)
            {
                return NotFound();
            }

            db.ChiTietSanPhams.Remove(product);
            db.SaveChanges();

            return Ok(new { success = true, message = "Sản phẩm đã được xóa." });

        }

        // thêm 1 sản phẩm (ADMIN)
        [HttpPost]
        [Route("add")]
        public IActionResult AddProduct([FromBody] Chitietsanpham product)
        {
            if (product == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                db.ChiTietSanPhams.Add(product); // Thêm sản phẩm vào DB
                db.SaveChanges();

                return Ok(new { success = true, message = "Thêm sản phẩm thành công.", data = product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Có lỗi xảy ra.", error = ex.Message });
            }
        }

        // chỉnh sửa sản phẩm (ADMIN)
        [HttpPut]
        [Route("edit")]
        public IActionResult EditProduct([FromBody] Chitietsanpham product)
        {
            if (product == null || product.MaSp <= 0) 
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                db.ChiTietSanPhams.Update(product); 
                db.SaveChanges();

                return Ok(new { success = true, message = "Chỉnh sửa sản phẩm thành công.", data = product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Có lỗi xảy ra.", error = ex.Message });
            }
        }


    }
}
