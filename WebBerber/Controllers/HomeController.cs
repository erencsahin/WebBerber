using Microsoft.AspNetCore.Mvc;
using WebBerber.Utils; // AppDbContext 

namespace WebBerber.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context; // DbContext

        public HomeController(AppDbContext context)
        {
            _context = context; 
        }

        public IActionResult Index()
        {
            var womenServices = _context.Operations.Where(o => o.Gender == "Kadýn").ToList();
            var menServices = _context.Operations.Where(o => o.Gender == "Erkek").ToList();

            ViewData["WomenServices"] = womenServices;
            ViewData["MenServices"] = menServices;

            return View(); 
        }

    }
}
