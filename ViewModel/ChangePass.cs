using System.ComponentModel.DataAnnotations;

namespace WebBanLapTop.ViewModel
{
    public class ChangePass
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Mật khẩu cũ không được để trống")]
        public String oldPassword { get; set; }
        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        public String newPassword { get; set; }
      

    }
}
