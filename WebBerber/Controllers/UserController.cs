using Microsoft.AspNetCore.Mvc;

namespace WebBerber.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
