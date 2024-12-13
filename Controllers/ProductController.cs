using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            Console.WriteLine(id);
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/DetailAPI/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Product not found" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

            var product = await response.Content.ReadFromJsonAsync<HomeVM>();
            return View(product); // Trả về View với thông tin sản phẩm
        }
    }
}
