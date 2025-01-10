using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;
using WebBanLapTop.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebBanLapTop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly LaptopShopContext db;

        public CheckoutController(LaptopShopContext context)
        {
            db = context;
        }

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("MYCART") ?? new List<CartItem>();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            decimal subtotal = (decimal)cart.Sum(p => p.totalPrice);

            decimal shippingFee = 100000;

            decimal estimatedTotal = subtotal + shippingFee;

            ViewBag.Subtotal = subtotal;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.EstimatedTotal = estimatedTotal;

            return View(cart);
        }
        [HttpPost]
        public IActionResult DatHang(string houseAddress, string ward, string district, string city, string sdt)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userNameClaim = HttpContext.User.Identity.Name;

            // Kiểm tra người dùng đã đăng nhập
            if (userNameClaim == null)
            {
                // Lấy URL hiện tại để quay lại sau khi đăng nhập
                var currentUrl = Url.Action("DatHang", "Checkout",
                    new { houseAddress, ward, district, city, sdt }, Request.Scheme);
                return RedirectToAction("SignIn", "Account", new { ReturnUrl = currentUrl });
            }


            int userId = int.Parse(userIdClaim.Value);
            
            string userName = userNameClaim;

            var cart = HttpContext.Session.Get<List<CartItem>>("MYCART") ?? new List<CartItem>();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var tongTien = (int)cart.Sum(p => p.totalPrice);

            string fullAddress = $"{houseAddress}, {ward}, {district}, {city}";

            var lastHoaDon = db.Hoadons.OrderByDescending(h => h.Id).FirstOrDefault();
            int newHoaDonId = (lastHoaDon?.Id ?? 0) + 1;

            var hoaDon = new Hoadon
            {
                Id = newHoaDonId,
                Iddn = userId, 
                TenDn = userName, 
                Sdt = sdt,
                NgayDat = DateTime.Now,
                DiaChiNhan = fullAddress,
                TongTien = tongTien,
                TrangThai = false 
            };

            db.Hoadons.Add(hoaDon);
            db.SaveChanges();

            foreach (var item in cart)
            {
                var chiTietHoaDon = new Chitiethoadon
                {
                    Idhd = hoaDon.Id,
                    MaSp = item.id,
                    SoLuong = item.quantity,
                    DonGia = (int)item.price 
                };

                db.Chitiethoadons.Add(chiTietHoaDon);
            }

            db.SaveChanges();

            HttpContext.Session.Remove("MYCART");

            return RedirectToAction("Success", "Checkout");
        }


        public IActionResult Success()
        {
            return View();
        }
    }
}