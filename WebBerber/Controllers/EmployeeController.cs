using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBerber.Filters;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext dbContext;
        public EmployeeController(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }

        //[EmployeeAuthorize]
        public IActionResult PendingAppointments()
        {
            int employeeId = GetLoggedInEmployeeId();

            var pendingAppointments = dbContext.Appointments
                .Where(a => a.EmployeeId == employeeId && !a.IsApproved)
                .Include(a => a.Operation)
                .ToList();
            return View(pendingAppointments);
        }


        public IActionResult GetStatistics()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");
            if (employeeId == null || employeeId == 0)
            {
                TempData["ErrorMessage"] = "Çalışan kimliği alınamadı.";
                return RedirectToAction("Login", "Home");
            }

            var appointments = dbContext.Appointments
                .Where(a => a.EmployeeId == employeeId && a.IsApproved)
                .ToList();

            var statistics = appointments
                .GroupBy(a => a.StartTime.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalOperations = g.Count(),
                    TotalEarnings = g.Sum(a => a.Price)
                })
                .OrderByDescending(s => s.Date)
                .ToList();

            return View(statistics);
        }



        private int GetLoggedInEmployeeId()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");

            if (employeeId == null)
            {
                throw new Exception("Çalışan oturum açmamış.");
            }
            return employeeId.Value;
        }
    }
}