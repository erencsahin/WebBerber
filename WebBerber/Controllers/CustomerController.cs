using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    public class CustomerController : Controller
    {

        private readonly AppDbContext appDbContext;

        public CustomerController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListShops()
        {
            var shops = appDbContext.Shops.ToList();
            return View(shops);
        }

        public IActionResult SelectOperation(int shopId)
        {
            var operations = appDbContext.Operations
                .Where(op => appDbContext.EmployeeOperations
                    .Any(eo => eo.OperationId == op.Id &&
                               appDbContext.Employees.Any(e => e.Id == eo.EmployeeId && e.ShopId == shopId)))
                .ToList();

            ViewBag.ShopId = shopId; // shopId'yi ViewBag ile gönderiyoruz
            return View(operations); // IEnumerable<Operation> modeli gönderiliyor
        }


        public IActionResult SelectEmployee(int shopId, int operationId)
        {
            var employees = appDbContext.Employees
                .Where(e => e.ShopId == shopId &&
                            appDbContext.EmployeeOperations.Any(eo => eo.EmployeeId == e.Id && eo.OperationId == operationId))
                .ToList();

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            return View(employees);
        }
    }
}
