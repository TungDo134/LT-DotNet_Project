using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;
using WebBanLapTop.Helpers;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;

        public CartSummaryViewComponent(LaptopShopContext context, IHttpClientFactory httpClientFactory)
        {
            db = context;
            _httpClientFactory = httpClientFactory;
        }

        public IViewComponentResult Invoke()
        {
            const string CART_KEY = "MYCART";
            var cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
            var totalQuantity = cart.Sum(item => item.quantity);

            return View(totalQuantity);
        }
    }
}
