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
            var operations = _context.Operations.Where(o => o.Gender == "Erkek").ToList();
            return View(operations); 
        }
    }
}
