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

        public IActionResult ListEmployees(int shopId)
        {
            var employees = appDbContext.Employees.Where(e => e.ShopId == shopId).ToList();
            return View(employees);
        }

        public IActionResult BookAppointment()
        {
            ViewBag.Employees=appDbContext.Employees.ToList();
            ViewBag.Operations=appDbContext.Operations.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult BookAppointment(Appointment model)
        {
            var isConflict = appDbContext.Appointments.Any(x =>
                x.EmployeeId == model.EmployeeId &&
                x.StartTime < model.StartTime.AddMinutes(model.Duration) &&
                x.StartTime.AddMinutes(x.Duration) > model.StartTime
            );

            if (isConflict)
            {
                ViewBag.ErrorMessage = "Çalışan bu saatte uygun değil.";
                ViewBag.Employees= appDbContext.Employees.ToList();
                ViewBag.Operations = appDbContext.Operations.ToList();
                return View(model);
            }


            var operation = appDbContext.Operations.Find(model.OperationId);
            appDbContext.Appointments.Add(model);
            appDbContext.SaveChanges();

            return RedirectToAction("ListShops");

        }

    }
}
