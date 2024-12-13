using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebBanLapTop.Data;
using WebBanLapTop.Models;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LaptopShopContext db;


        public HomeController(ILogger<HomeController> logger, LaptopShopContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {

            var list_pro_home = db.ChiTietSanPhams.AsQueryable();

            var list = list_pro_home.Select(p => new HomeVM
            {
                Id = p.MaSp,
                Name = p.TenSp ?? "",
                Img = p.HinhAnh ?? "",
                Price = p.DonGia ?? 0

            }); 
            return View(list);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
