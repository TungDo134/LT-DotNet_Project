using System;
using System.Collections.Generic;

namespace WebBanLapTop.Data;

public partial class ChiTietSanPham
{
    public int MaSp { get; set; }

    public int? MaDanhMuc { get; set; }

    public string? TenSp { get; set; }

    public int? DonGia { get; set; }

    public string? KhuyenMai { get; set; }

    public string? ThongTinSp { get; set; }

    public decimal? KhoiLuong { get; set; }

    public string? HinhAnh { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual DanhMucSanPham? MaDanhMucNavigation { get; set; }
}
