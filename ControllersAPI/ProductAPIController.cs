using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                    KhoiLuong = p.KhoiLuong ?? 0,
                    MaDanhMuc = p.MaDanhMuc ?? 0,
                    TenDanhMuc = p.MaDanhMucNavigation.TenDanhMuc ?? ""
                });


            // Kiểm tra nếu không tìm thấy sản phẩm
            if (products == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Trả về sản phẩm nếu tìm thấy
            return Ok(products);
        }


        // tìm kiếm theo keyword
        [HttpGet("search/{txt}")]
        public IActionResult Search(string txt)
        {
            if (string.IsNullOrWhiteSpace(txt))
            {
                return BadRequest(new { message = "Search text cannot be empty" });
            }

            // Chuyển đổi từ khóa sang chữ thường một lần
            var lowerTxt = txt.ToLower();

            // Tìm kiếm trong danh sách sản phẩm
            var products = db.ChiTietSanPhams
                .Where(p => p.TenSp != null && p.TenSp.ToLower().Contains(lowerTxt))
                .Select(p => new ProductVM
                {
                    Id = p.MaSp,
                    Name = p.TenSp ?? "",
                    Img = p.HinhAnh ?? "",
                    Price = p.DonGia ?? 0,
                    Description = p.ThongTinSp ?? "",
                    KhoiLuong = p.KhoiLuong ?? 0,
                })
                .ToList();

            // Kiểm tra nếu không tìm thấy sản phẩm
            if (!products.Any())
            {
                return NotFound(new { message = "No products found matching the search term." });
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


        // sắp xếp sản phẩm
        [HttpGet]
        [Route("sort/{code}")]
        public IActionResult Sort(int code)
        {
            IQueryable<Chitietsanpham> products = db.ChiTietSanPhams;

            // Thực hiện sắp xếp dựa trên code
            switch (code)
            {
                case 1: // Giá tăng dần
                    products = products.OrderBy(p => p.DonGia);
                    break;

                case 2: // Giá giảm dần
                    products = products.OrderByDescending(p => p.DonGia);
                    break;

                case 3: // Tên A-Z
                    products = products.OrderBy(p => p.TenSp);
                    break;

                case 4: // Tên Z-A
                    products = products.OrderByDescending(p => p.TenSp);
                    break;

                default: // Code không hợp lệ
                    return BadRequest(new { message = "Invalid sort code. Valid codes are 1, 2, 3, 4." });
            }

            
            var productVMs = products
                .Select(p => new ProductVM
                {
                    Id = p.MaSp,
                    Name = p.TenSp ?? "",
                    Img = p.HinhAnh ?? "",
                    Price = p.DonGia ?? 0,
                    Description = p.ThongTinSp ?? "",
                    KhoiLuong = p.KhoiLuong ?? 0,
                })
                .ToList();

            return Ok(productVMs);
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
