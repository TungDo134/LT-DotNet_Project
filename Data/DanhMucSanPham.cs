using System;
using System.Collections.Generic;

namespace WebBanLapTop.Data;

public partial class DanhMucSanPham
{
    public int MaDanhMuc { get; set; }

    public string? TenDanhMuc { get; set; }

    public virtual ICollection<ChiTietSanPham> ChiTietSanPhams { get; set; } = new List<ChiTietSanPham>();
}
