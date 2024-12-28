using Microsoft.AspNetCore.Mvc;
using WebBerber.Api.Abstract;
using WebBerber.Models;

namespace WebBerber.Api.Concrete
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopApiController : ControllerBase
    {
        private readonly IRepository<Shop> _shopRepository;

        public ShopApiController(IRepository<Shop> shopRepository)
        {
            _shopRepository = shopRepository;
        }

        [HttpGet("{shopId}")]
        public IActionResult GetShopById(int shopId)
        {
            var shop = _shopRepository.GetById(shopId);

            if (shop == null)
            {
                return NotFound("Böyle bir dükkan bulunamadı.");
            }

            return Ok(shop);
        }

        [HttpGet]
        public IActionResult GetAllShops()
        {
            var shops = _shopRepository.GetAll();
            return Ok(shops);
        }
    }

}
