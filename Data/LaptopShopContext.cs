using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebBanLapTop.Data;

public partial class LaptopShopContext : DbContext
{
    public LaptopShopContext()
    {
    }

    public LaptopShopContext(DbContextOptions<LaptopShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietSanPham> ChiTietSanPhams { get; set; }

    public virtual DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=TUNGDO\\SQLEXPRESS;Initial Catalog=laptop_shop;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.Idhd, e.MaSp }).HasName("pk");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.Idhd).HasColumnName("IDHD");
            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.DonGia).HasColumnName("donGia");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.IdhdNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.Idhd)
                .HasConstraintName("fk_ChiTietHoaDon");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaSp)
                .HasConstraintName("fk_ChiTietHoaDonSP");
        });

        modelBuilder.Entity<ChiTietSanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__ChiTietS__7A227A7A44FF419A");

            entity.ToTable("ChiTietSanPham");

            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.DonGia).HasColumnName("donGia");
            entity.Property(e => e.HinhAnh).HasColumnName("hinhAnh");
            entity.Property(e => e.KhoiLuong)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("khoiLuong");
            entity.Property(e => e.KhuyenMai).HasColumnName("khuyenMai");
            entity.Property(e => e.MaDanhMuc).HasColumnName("maDanhMuc");
            entity.Property(e => e.TenSp)
                .HasMaxLength(250)
                .HasColumnName("tenSP");
            entity.Property(e => e.ThongTinSp)
                .HasColumnType("ntext")
                .HasColumnName("thongTinSP");

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.ChiTietSanPhams)
                .HasForeignKey(d => d.MaDanhMuc)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_ChiTietSanPham_LoaiMyPham");
        });

        modelBuilder.Entity<DanhMucSanPham>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PK__DanhMucS__6B0F914C6F8D8266");

            entity.ToTable("DanhMucSanPham");

            entity.Property(e => e.MaDanhMuc).HasColumnName("maDanhMuc");
            entity.Property(e => e.TenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("tenDanhMuc");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HoaDon__3214EC2742E1EEFE");

            entity.ToTable("HoaDon");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DiaChiNhan)
                .HasMaxLength(30)
                .HasColumnName("diaChiNhan");
            entity.Property(e => e.Iddn).HasColumnName("IDDN");
            entity.Property(e => e.NgayDat)
                .HasColumnType("datetime")
                .HasColumnName("ngayDat");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("sdt");
            entity.Property(e => e.TenDn)
                .HasMaxLength(20)
                .HasColumnName("tenDN");
            entity.Property(e => e.TongTien).HasColumnName("tongTien");
            entity.Property(e => e.TrangThai).HasColumnName("trangThai");

            entity.HasOne(d => d.IddnNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.Iddn)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_HoaDon_KH");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iddn).HasName("PK__Users__B87DB89243224B74");

            entity.Property(e => e.Iddn).HasColumnName("IDDN");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(30)
                .HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.MatkhauDn)
                .HasMaxLength(30)
                .HasColumnName("matkhauDN");
            entity.Property(e => e.Quyen).HasColumnName("quyen");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("sdt");
            entity.Property(e => e.TenDn)
                .HasMaxLength(30)
                .HasColumnName("tenDN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
