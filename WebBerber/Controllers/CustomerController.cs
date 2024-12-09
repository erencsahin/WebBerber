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

        public IActionResult BookAppointment(int shopId)
        {
            ViewBag.Employees=appDbContext.Employees.Where(e=>e.ShopId == shopId).ToList();
            ViewBag.Operations=appDbContext.Operations.ToList();
            ViewBag.ShopId=shopId;
            return View();
        }

        [HttpPost]
        public IActionResult BookAppointment(Appointment model,int shopId)
        {
            var isConflict = appDbContext.Appointments.Any(x =>
                x.EmployeeId == model.EmployeeId &&
                x.StartTime < model.StartTime.AddMinutes(model.Duration) &&
                x.StartTime.AddMinutes(x.Duration) > model.StartTime
            );

            if (isConflict)
            {
                ViewBag.ErrorMessage = "Çalışan bu saatte uygun değil.";
                ViewBag.Employees = appDbContext.Employees.Where(e => e.ShopId == shopId).ToList();
                ViewBag.Operations = appDbContext.Operations.ToList();
                ViewBag.ShopId = shopId;
                return View(model);
            }

            model.IsApproved = false;

            appDbContext.Appointments.Add(model);
            appDbContext.SaveChanges();

            return RedirectToAction("ListShops");

        }

    }
}
