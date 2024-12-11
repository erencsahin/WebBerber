using Microsoft.AspNetCore.Mvc;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    [Route("api/[controller]")]
    public class ShopApiController : Controller
    {
        private readonly AppDbContext appDbContext;

        public ShopApiController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetShops()
        {
            var shops = appDbContext.Shops.Select(x => new
            {
                x.ShopName,
                x.Address,
            }).ToList();

            return Ok(shops);
        }


        [HttpGet("{id}")]
        public IActionResult GetShopById(int shopId) 
        {
            var shop=appDbContext.Shops.Where(x=>x.Id == shopId).
                Select(x => new { x.ShopName, x.Address}).FirstOrDefault();

            if (shop != null)
            {
                return NotFound("Böyle bir dükkan bulunamadı.");
            }
            return Ok(shop);
        }
    }
}
