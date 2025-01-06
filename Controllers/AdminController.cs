using Microsoft.AspNetCore.Mvc;

namespace WebBanLapTop.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
