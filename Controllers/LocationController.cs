using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

public class LocationController : Controller
{
    public async Task<IActionResult> Index()
    {
        // URL chứa dữ liệu JSON
        string url = "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                // Gửi yêu cầu HTTP GET để lấy dữ liệu
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Kiểm tra nếu yêu cầu thành công

                // Đọc nội dung JSON
                string jsonData = await response.Content.ReadAsStringAsync();

                // Truyền dữ liệu JSON đến View hoặc Deserialize nếu cần
                ViewBag.JsonData = jsonData;
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Lỗi khi tải dữ liệu: {ex.Message}";
        }

        return View();
    }
}
