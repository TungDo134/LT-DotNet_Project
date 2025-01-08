using System;
using System.Collections.Generic;

namespace WebBanLapTop.Data;

public partial class Danhmucsanpham
{
    public int MaDanhMuc { get; set; }

    public string? TenDanhMuc { get; set; }

    public string? HinhDanhMuc { get; set; }

    public virtual ICollection<Chitietsanpham> Chitietsanphams { get; set; } = new List<Chitietsanpham>();
}
