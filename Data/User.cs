using System;
using System.Collections.Generic;

namespace WebBanLapTop.Data;

public partial class User
{
    public int Iddn { get; set; }

    public string? TenDn { get; set; }

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    public string? Sdt { get; set; }

    public string? MatkhauDn { get; set; }

    public bool? Quyen { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
