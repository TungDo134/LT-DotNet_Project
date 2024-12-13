using System;
using System.Collections.Generic;

namespace WebBanLapTop.Data;

public partial class ChiTietHoaDon
{
    public int Idhd { get; set; }

    public int MaSp { get; set; }

    public int? DonGia { get; set; }

    public int? SoLuong { get; set; }

    public virtual HoaDon IdhdNavigation { get; set; } = null!;

    public virtual ChiTietSanPham MaSpNavigation { get; set; } = null!;
}
