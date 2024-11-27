using Microsoft.AspNetCore.Mvc;

namespace WebBerber.Controllers
{
    public class OperationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
