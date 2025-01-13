using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebBanLapTop.Data;
using WebBanLapTop.ViewModel;

namespace WebBanLapTop.Controllers
{

    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly LaptopShopContext db;
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(ILogger<AdminController> logger, LaptopShopContext context, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            db = context;
            _httpClientFactory = httpClientFactory;
        }




        public IActionResult Index()
        {
            return View();
        }


        // lay ra ds sp de hien thi cho admin
        public async Task<IActionResult> listProduct()
        {
            var client = _httpClientFactory.CreateClient();

            // Gọi API để lấy danh sách sản phẩm
            // responseP chứa kết quả phản hồi từ API
            var responseP = await client.GetAsync($"https://localhost:7258/api/ProductAPI");
            IEnumerable<ProductVM> products = null;

            if (responseP.IsSuccessStatusCode)
            {
                var jsonResponseP = await responseP.Content.ReadAsStringAsync();
                products = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(jsonResponseP);
            }

            return View(products);
        }

        // Hiển thị trang thêm sản phẩm và xử lí sk thêm sp
        public async Task<IActionResult> AddProduct()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            // Gọi API để lấy danh sách danh mục
            var responseC = await client.GetAsync($"https://localhost:7258/api/CateAPI");
            IEnumerable<CateVM> categories = null;

            if (responseC.IsSuccessStatusCode)
            {
                var jsonResponseC = await responseC.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<IEnumerable<CateVM>>(jsonResponseC);

            }

            return View(categories);
        }

        // Hiển thị trang chỉnh sửa sản phẩm 
        public async Task<IActionResult> EditProduct(int id)
        {
            
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/DetailAPI/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Khong tim thay san pham" }); // Xử lý lỗi nếu không tìm thấy sản phẩm
            }

            var product = await response.Content.ReadFromJsonAsync<ProductVM>();
            return View(product); // Trả về View với thông tin sản phẩm

           
        }





        // ds the loai
        public IActionResult ListCategory()
        {
            var categories = db.Danhmucsanphams
                .Select(c => new CateVM
                {
                    Id = c.MaDanhMuc,
                    Name = c.TenDanhMuc,
                    Hinh = c.HinhDanhMuc
                })
                .ToList();

            return View(categories); // Trả về view ListCategory với danh sách danh mục
        }



        // them the loai

        public IActionResult ShowAddCate()
        {

            return View();

        }
        public IActionResult AddCate(String name, String cateImg, String cateID)
        {
            // Lấy giá trị từ form
            String _name = name;
            String _hinh = cateImg;
            String _id = cateID;
            int CateId = 0;

            // Kiểm tra nếu cateID không rỗng thì parse sang int
            if (_id != null)
            {
                CateId = Int32.Parse(_id);
            }

            // Kiểm tra nếu danh mục đã tồn tại trong cơ sở dữ liệu (theo tên hoặc ID)
            var existingCategory = db.Danhmucsanphams.FirstOrDefault(c => c.TenDanhMuc == _name || c.MaDanhMuc == CateId);

            if (existingCategory != null)
            {
                // Nếu danh mục đã tồn tại, trả về thông báo lỗi
                ViewBag.ErrorMessage = "Danh mục này đã tồn tại!";
                return View();  // Trả về lại view AddCate và hiển thị thông báo lỗi
            }

            // Nếu danh mục chưa tồn tại, tiến hành thêm mới
            Danhmucsanpham data = new Danhmucsanpham
            {
                MaDanhMuc = CateId,
                TenDanhMuc = _name,
                HinhDanhMuc = _hinh ?? ""
            };

            db.Danhmucsanphams.Add(data);
            db.SaveChanges();

            // Sau khi thêm xong, chuyển về danh sách danh mục
            return RedirectToAction("ListCategory");
        }



        //xoa
        public IActionResult DeleteCate(int id)
        {
            Danhmucsanpham data = db.Danhmucsanphams.Find(id);

            db.Danhmucsanphams.Remove(data);
            db.SaveChanges();


            return RedirectToAction("ListCategory");
        }

        //edit cate
        public IActionResult ShowEditCate(int id)
        {
            var category = db.Danhmucsanphams
                .Where(c => c.MaDanhMuc == id)
                .Select(c => new CateVM
                {
                    Id = c.MaDanhMuc,
                    Name = c.TenDanhMuc,
                    Hinh = c.HinhDanhMuc // Chuyển c.Image thành Hinh trong ViewModel
                })
                .FirstOrDefault(); // Lấy một đối tượng đơn lẻ

            if (category == null)
            {
                return NotFound("Danh mục không tồn tại");
            }

            return View(category); // Truyền đối tượng category vào view
        }



        public IActionResult EditCate(int id)
        {
            // Tìm danh mục cần chỉnh sửa theo ID
            var category = db.Danhmucsanphams
                .Where(c => c.MaDanhMuc == id)
                .Select(c => new CateVM
                {
                    Id = c.MaDanhMuc,
                    Name = c.TenDanhMuc,
                    Hinh = c.HinhDanhMuc
                })
                .FirstOrDefault();

            if (category == null)
            {
                return NotFound("Danh mục không tồn tại");
            }

            // Truyền thông tin danh mục sang View
            return View(category);
        }


        public IActionResult SaveEditCate(CateVM model)
        {
            if (!ModelState.IsValid)
            {
                // Kiểm tra các lỗi trong ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine("Error: " + error.ErrorMessage);
               }

                return View("ShowEditCate", model);  // Trả lại view với thông báo lỗi
            }

            // Tìm danh mục cần chỉnh sửa
            var category = db.Danhmucsanphams.Find(model.Id);
            if (category == null)
            {
                return NotFound("Danh mục không tồn tại");
            }

            category.TenDanhMuc = model.Name;

            if (model.Hinh != null) 
            {
                // Lưu ảnh nếu cần (hoặc để null nếu không cần)
                // category.HinhDanhMuc = UploadFile(model.CateImg); // Tạm thời không xử lý ảnh
            }
            else
            {
                category.HinhDanhMuc = null; 
            }
            db.Danhmucsanphams.Update(category);
            db.SaveChanges();

            return RedirectToAction("ListCategory");
        }







        // Lấy danh sách đơn hàng
        public async Task<IActionResult> ListOrder()
        {
            var client = _httpClientFactory.CreateClient();

            // Gọi API để lấy danh sách đơn hàng
            var response = await client.GetAsync("https://localhost:7258/api/OrderAPI");
            IEnumerable<Hoadon> orders = null;

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                orders = JsonConvert.DeserializeObject<IEnumerable<Hoadon>>(jsonResponse);
            }
            else
            {
                ViewBag.ErrorMessage = $"Không thể tải danh sách đơn hàng. Mã lỗi: {response.StatusCode}";
                orders = new List<Hoadon>(); // Trả về danh sách rỗng nếu không thành công
            }

            return View(orders);
        }
        //chi tiet hoa don
        public async Task<IActionResult> OrderDetail(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7258/api/OrderAPI/detail/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error", new { Message = "Không tìm thấy đơn hàng" });
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Sử dụng JsonSerializer để giải mã JSON vào đối tượng OrderDetailVM
            var orderDetail = JsonConvert.DeserializeObject<OrderDetailVM>(jsonResponse);

            return View(orderDetail);
        }


        // ds ng dung
        public IActionResult listUser()
        {
            return View();
        }

        // them ng dung
        public IActionResult addUser()
        {
            return View();
        }
    }

    internal class DanhMucSanPham
    {
        internal int MaDanhMuc;
        internal string TenDanhMuc;
        internal string HinhDanhMuc;
    }
}
