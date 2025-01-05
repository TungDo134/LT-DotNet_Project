using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.ViewComponents
{

    public class CategoryViewComponent : ViewComponent
    {
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;
        public CategoryViewComponent(LaptopShopContext context, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           

            var client = _httpClientFactory.CreateClient();

            // Gọi API để lấy danh sách danh mục
            var responseC = await client.GetAsync($"https://localhost:7258/api/CateAPI");
           
            if (!responseC.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Category  not found" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

            var categories = await responseC.Content.ReadFromJsonAsync<List<CateVM>>();
            return View(categories);
            // Trả về View với thông tin sản phẩm

            // mặc định sử dụng "Default.cshtml" 
            /* hoặc có thể tự đặt tên view 
            // return ("ten view", data)
            / */
        }



    }
}
