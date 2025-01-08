using System.ComponentModel.DataAnnotations;

namespace WebBanLapTop.ViewModel
{
    public class ResetPasswordVM
    {
        public string Token { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
