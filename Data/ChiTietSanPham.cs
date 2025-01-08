using System;
using System.Collections.Generic;

namespace WebBanLapTop.Data;

public partial class Chitietsanpham
{
    public int MaSp { get; set; }

    public int? MaDanhMuc { get; set; }

    public string? TenSp { get; set; }

    public decimal? DonGia { get; set; }

    public string? KhuyenMai { get; set; }

    public string? ThongTinSp { get; set; }

    public decimal? KhoiLuong { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<Chitiethoadon> Chitiethoadons { get; set; } = new List<Chitiethoadon>();

    public virtual Danhmucsanpham? MaDanhMucNavigation { get; set; }
}
