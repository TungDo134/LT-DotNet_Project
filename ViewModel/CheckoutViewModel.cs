using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBanLapTop.ViewModel
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên của bạn")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^(\+84|0)\d{9,10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập phường/xã")]
        public string Ward { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập quận/huyện")]
        public string District { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tỉnh/thành phố")]
        public string City { get; set; }

        public decimal TotalAmount { get; set; }

        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
    }
}
