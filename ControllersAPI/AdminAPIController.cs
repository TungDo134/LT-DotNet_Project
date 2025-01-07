using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;

namespace WebBanLapTop.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {

        private readonly LaptopShopContext db;
        public AdminAPIController(LaptopShopContext context)
        {
            db = context;
        }


        // xoa sp dua tren id
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


    }

}
