using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;
using WebBanLapTop.ViewModels;

namespace WebBanLapTop.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAPIController : ControllerBase
    {
        private readonly LaptopShopContext _context;

        public OrderAPIController(LaptopShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Hoadons
                .Select(o => new Hoadon
                {
                    Id = o.Id,
                    Iddn = o.Iddn,
                    TenDn = o.TenDn,
                    Sdt = o.Sdt,
                    NgayDat = o.NgayDat,
                    DiaChiNhan = o.DiaChiNhan,
                    TongTien = o.TongTien,
                    TrangThai = o.TrangThai
                })
                .ToList();

            return Ok(orders);  // Trả về kết quả dưới dạng JSON
        }
        [HttpGet("detail/{id}")]
        public IActionResult GetOrderDetail(int id)
        {
            var orderDetail = _context.Hoadons
                .Where(h => h.Id == id)
                .Select(o => new OrderDetailVM
                {
                    Id = o.Id,
                    TenDn = o.TenDn,
                    Sdt = o.Sdt,
                    NgayDat = o.NgayDat,
                    DiaChiNhan = o.DiaChiNhan,
                    TongTien = (int)o.TongTien,
                    TrangThai = (bool)o.TrangThai,
                    Products = o.Chitiethoadons.Select(c => new ProductDetailVM
                    {
                        MaSanPham = c.MaSp,
                        TenSp = c.MaSpNavigation.TenSp,
                        HinhAnh = c.MaSpNavigation.HinhAnh,
                        DonGia = (int)c.DonGia,
                        SoLuong =(int) c.SoLuong
                    }).ToList()
                })
                .FirstOrDefault();

            if (orderDetail == null)
            {
                return NotFound();  // Nếu không tìm thấy đơn hàng
            }

            return Ok(orderDetail);  // Trả về OrderDetailVM
        }


    }
}
