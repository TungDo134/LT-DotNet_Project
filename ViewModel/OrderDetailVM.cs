public class OrderDetailVM
{
    public int Id { get; set; }
    public int IdDn { get; set; }
    public string TenDn { get; set; }
    public string Sdt { get; set; }
    public DateTime? NgayDat { get; set; }
    public string DiaChiNhan { get; set; }
    public int TongTien { get; set; }
    public bool TrangThai { get; set; }

    public List<ProductDetailVM> Products { get; set; }
}

public class ProductDetailVM
{
    public int MaSanPham { get; set; }
    public string TenSp { get; set; }  
    public string HinhAnh { get; set; }
    public int DonGia { get; set; }
    public int SoLuong { get; set; }
}
