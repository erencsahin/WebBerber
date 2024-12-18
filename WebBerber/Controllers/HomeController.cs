using Microsoft.AspNetCore.Mvc;
using WebBerber.Utils; // AppDbContext 
using WebBerber.Models;
using System.Linq;

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
            var operations = _context.Operations.ToList(); 
            return View(operations); 
        }
    }
}
