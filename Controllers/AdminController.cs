using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;

namespace WebBanLapTop.Controllers
{

    public class AdminController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(ILogger<HomeController> logger, LaptopShopContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            db = context;
            _httpClientFactory = httpClientFactory;
        }




        public IActionResult Index()
        {
            return View();
        }


        // ds sp
        public IActionResult listProduct()
        {
            return View();
        }

        // them sp
        public IActionResult addProduct()
        {
            return View();
        }


        // ds the loai
        public IActionResult listCategory()
        {
            return View();
        }

        // them the loai
        public IActionResult addCategory()
        {
            return View();
        }

        // ds don hang
        public IActionResult listOrder()
        {
            return View();
        }

        // ctdh
        public IActionResult orderDetail()
        {
            return View();
        }

        // ds ng dung
        public IActionResult listUser()
        {
            return View();
        }

        // them ng dung
        public IActionResult addUser()
        {
            return View();
        }
    }
}
