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

        public IActionResult Shops()
        {
            var shops=_context.Shops.ToList();
            return View(shops);
        }


        public IActionResult Ekip(int shopId)
        {
            var shop = _context.Shops.FirstOrDefault(s => s.Id == shopId);

            if (shop == null)
            {
                TempData["ErrorMessage"] = "Seçilen dükkana ait bilgi bulunamadý.";
                return RedirectToAction("Shops");
            }

            var employees = _context.Employees
                .Where(e => e.ShopId == shopId)
                .ToList();

            var employeeOperations = _context.EmployeeOperations
                .Where(eo => employees.Select(e => e.Id).Contains(eo.EmployeeId))
                .ToList();

            var operations = _context.Operations.ToList();

            ViewBag.ShopName = shop.ShopName;
            ViewBag.ShopAddress = shop.Address;
            ViewBag.Employees = employees;
            ViewBag.EmployeeOperations = employeeOperations;
            ViewBag.Operations = operations;

            return View();
        }

        public IActionResult Operations()
        {
            var ops = _context.Operations.ToList();
            return View(ops);
        }

    }
}
