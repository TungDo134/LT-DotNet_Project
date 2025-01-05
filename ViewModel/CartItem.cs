namespace WebBanLapTop.ViewModel
{
    public class CartItem
    {
        public int id { get; set; }
        public string image { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double totalPrice => price * quantity;
    }
}
