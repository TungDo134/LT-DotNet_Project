namespace WebBanLapTop.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Pass { get; set; }
        public bool? Role { get; set; }

        // Thêm thuộc tính này chỉ để hiển thị ở View
        public string RoleString => Role == true ? "Admin" : "Người dùng";
    }

}
