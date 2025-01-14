using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

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

    public virtual DbSet<Chitiethoadon> Chitiethoadons { get; set; }

    public virtual DbSet<Chitietsanpham> ChiTietSanPhams { get; set; }

    public virtual DbSet<Danhmucsanpham> Danhmucsanphams { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=laptop_shop;user=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.30-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_as_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Chitiethoadon>(entity =>
        {
            entity.HasKey(e => new { e.Idhd, e.MaSp })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("chitiethoadon");

            entity.HasIndex(e => e.MaSp, "fk_ChiTietHoaDonSP");

            entity.Property(e => e.Idhd).HasColumnName("IDHD");
            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.DonGia).HasColumnName("donGia");
            entity.Property(e => e.SoLuong).HasColumnName("soLuong");

            entity.HasOne(d => d.IdhdNavigation).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.Idhd)
                .HasConstraintName("fk_ChiTietHoaDon");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.Chitiethoadons)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ChiTietHoaDonSP");
        });

        modelBuilder.Entity<Chitietsanpham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PRIMARY");

            entity.ToTable("chitietsanpham");

            entity.HasIndex(e => e.MaDanhMuc, "fk_ChiTietSanPham_LoaiMyPham");

            entity.Property(e => e.MaSp).HasColumnName("maSP");
            entity.Property(e => e.DonGia)
                .HasPrecision(10, 2)
                .HasColumnName("donGia");
            entity.Property(e => e.HinhAnh).HasColumnName("hinhAnh");
            entity.Property(e => e.KhoiLuong)
                .HasPrecision(5, 2)
                .HasColumnName("khoiLuong");
            entity.Property(e => e.KhuyenMai).HasColumnName("khuyenMai");
            entity.Property(e => e.MaDanhMuc).HasColumnName("maDanhMuc");
            entity.Property(e => e.TenSp)
                .HasMaxLength(250)
                .HasColumnName("tenSP");
            entity.Property(e => e.ThongTinSp).HasColumnName("thongTinSP");

            entity.HasOne(d => d.MaDanhMucNavigation).WithMany(p => p.Chitietsanphams)
                .HasForeignKey(d => d.MaDanhMuc)
                .HasConstraintName("fk_ChiTietSanPham_LoaiMyPham");
        });

        modelBuilder.Entity<Danhmucsanpham>(entity =>
        {
            entity.HasKey(e => e.MaDanhMuc).HasName("PRIMARY");

            entity.ToTable("danhmucsanpham");

            entity.Property(e => e.MaDanhMuc)
                .ValueGeneratedNever()
                .HasColumnName("maDanhMuc");
            entity.Property(e => e.HinhDanhMuc)
                .HasMaxLength(255)
                .HasColumnName("hinhDanhMuc");
            entity.Property(e => e.TenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("tenDanhMuc");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("hoadon");

            entity.HasIndex(e => e.Iddn, "fk_HoaDon_KH");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DiaChiNhan)
                .HasMaxLength(300)
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

            entity.HasOne(d => d.IddnNavigation).WithMany(p => p.Hoadons)
                .HasForeignKey(d => d.Iddn)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_HoaDon_KH");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iddn).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.Iddn)
                .ValueGeneratedNever()
                .HasColumnName("IDDN");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(255)
                .HasColumnName("diaChi");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.MatkhauDn)
                .HasMaxLength(255)
                .HasColumnName("matkhauDN");
            entity.Property(e => e.Quyen).HasColumnName("quyen");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("sdt");
            entity.Property(e => e.TenDn)
                .HasMaxLength(255)
                .HasColumnName("tenDN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
