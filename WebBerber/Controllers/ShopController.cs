using Microsoft.AspNetCore.Mvc;

namespace WebBerber.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
