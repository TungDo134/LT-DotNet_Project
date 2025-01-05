using Microsoft.AspNetCore.Mvc;

namespace WebBanLapTop.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
