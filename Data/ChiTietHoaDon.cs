using System;
using System.Collections.Generic;

namespace WebBanLapTop.Data;

public partial class Chitiethoadon
{
    public int Idhd { get; set; }

    public int MaSp { get; set; }

    public int? DonGia { get; set; }

    public int? SoLuong { get; set; }

    public virtual Hoadon IdhdNavigation { get; set; } = null!;

    public virtual Chitietsanpham MaSpNavigation { get; set; } = null!;
}
