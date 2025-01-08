using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.Helpers;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{
    public class CartController : Controller
    {

        private readonly LaptopShopContext db;

        public CartController(LaptopShopContext context)
        {
            db = context;
        }

        const string CART_KEY = "MYCART";

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(CART_KEY)
            ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.id == id);
            if (item == null)
            {
                var _itemAdd = db.ChiTietSanPhams.SingleOrDefault(p => p.MaSp == id);

                if (_itemAdd != null)
                {

                    item = new CartItem
                    {
                        id = _itemAdd.MaSp,
                        image = _itemAdd.HinhAnh ?? String.Empty,
                        name = _itemAdd.TenSp ?? String.Empty,
                        price = (double)(_itemAdd.DonGia ?? 0),
                        quantity = quantity
                    };
                    cart.Add(item);

                }
            }
            else
            {
                item.quantity += quantity;
            }

            HttpContext.Session.Set(CART_KEY, cart);

            return RedirectToAction("Index");
            //return new EmptyResult();
        }

        public IActionResult RemoveCart(int id)
        {

            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.id == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult UpdateCart(int id, int quantity)
        {

            var cart = Cart;
            var item = cart.SingleOrDefault(p => p.id == id);
            if (item != null)
            {
                item.quantity = quantity;
                HttpContext.Session.Set(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }
    }
}
