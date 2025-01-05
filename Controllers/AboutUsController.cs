using Microsoft.AspNetCore.Mvc;

namespace WebBanLapTop.Controllers
{
    public class AboutUsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
