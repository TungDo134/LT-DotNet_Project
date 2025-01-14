using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

            var product = await response.Content.ReadFromJsonAsync<ProductVM>();
            return View(product); // Trả về View với thông tin sản phẩm
        }


        public async Task<IActionResult> ProductByCate(int id)
        {

            var client = _httpClientFactory.CreateClient();

            // Gọi API để lấy danh sách sản phẩm
            var responseP = await client.GetAsync($"https://localhost:7258/api/DetailAPI/pByCid/{id}");
            IEnumerable<ProductVM> products = null;

            if (responseP.IsSuccessStatusCode)
            {
                var jsonResponseP = await responseP.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(jsonResponseP);
            }

            // Gọi API để lấy danh sách danh mục
            var responseC = await client.GetAsync($"https://localhost:7258/api/CateAPI");
            IEnumerable<CateVM> categories = null;

            if (responseC.IsSuccessStatusCode)
            {
                var jsonResponseC = await responseC.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<IEnumerable<CateVM>>(jsonResponseC);
            }

            // Tạo ViewModel và truyền dữ liệu vào View
            var viewModel = new HomeVM
            {
                Products = products ?? new List<ProductVM>(),
                Categories = categories ?? new List<CateVM>()
            };

            return View(viewModel);
        }


        // search
        public async Task<IActionResult> search(String txt)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/ProductAPI/search/{txt}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Product not found" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(jsonResponse);



            return View(products);
        }


        // sort
        public async Task<IActionResult> Sort(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/ProductAPI/sort/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Product not found" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(jsonResponse);


            return View(products);
        }

    }
}
