using Microsoft.AspNetCore.Mvc;
using WebBanLapTop.Data;

namespace WebBanLapTop.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {

        private readonly LaptopShopContext db;
        public AdminAPIController(LaptopShopContext context)
        {
            db = context;
        }


        

    }

}
