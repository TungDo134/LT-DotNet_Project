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

        // hien thi danh muc
        [HttpGet]
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


        // hien thi danh muc theo id
        [HttpGet]
        [Route("getById/{id}")]
        public IActionResult GetById(int id)
        {
            // lay ra danh muc 
            var cate = db.Danhmucsanphams.Find(id);

           
            // Kiểm tra nếu không tìm thấy sản phẩm
            if (cate == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Trả về sản phẩm nếu tìm thấy
            return Ok(cate);
        }


        // xóa danh muc
        [HttpDelete]
        [Route("deleteCate/{id}")]
        public IActionResult Delete(int id) {
            Danhmucsanpham data = db.Danhmucsanphams.Find(id);

            db.Danhmucsanphams.Remove(data);
            db.SaveChanges();

            return Ok(new { success = true, message = "Danh mục đã được xóa." });
        
        }


        // them danh muc
        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] Danhmucsanpham cate)
        {
           
            if (cate == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                db.Danhmucsanphams.Add(cate); // Thêm sản phẩm vào DB
                db.SaveChanges();

                return Ok(new { success = true, message = "Thêm danh muc thành công.", data = cate });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Có lỗi xảy ra.", error = ex.Message });
            }

        }


        // cap nhat danh muc
        [HttpPut]
        [Route("update")]
        public IActionResult Update([FromBody] Danhmucsanpham cate)
        {

            if (cate == null)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                db.Danhmucsanphams.Update(cate); // Cập nhật sản phẩm vào DB
                db.SaveChanges();

                return Ok(new { success = true, message = " danh muc thành công.", data = cate });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Có lỗi xảy ra.", error = ex.Message });
            }

        }

    }
}
