using Microsoft.AspNetCore.Mvc;

namespace WebBanLapTop.Controllers
{
    public class AdminController : Controller
    {
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
