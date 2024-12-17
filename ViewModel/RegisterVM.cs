using System.ComponentModel.DataAnnotations;

namespace WebBanLapTop.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public string TenDn { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^0\d{8,10}$", ErrorMessage = "Phone number must start with 0 and be between 9 and 11 digits.")]
        [Display(Name = "Phone Number")]
        public string Sdt { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[\W_]).{6,}$", ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
        [Display(Name = "Password")]

        public string MatkhauDn { get; set; }
        // Thêm thuộc tính ConfirmPassword để so sánh với MatkhauDn
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("MatkhauDn", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
