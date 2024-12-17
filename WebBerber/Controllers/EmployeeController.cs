using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBerber.Filters;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    [EmployeeAuthorize]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext dbContext;
        public EmployeeController(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }

        public IActionResult PendingAppointments()
        {
            int employeeId = GetLoggedInEmployeeId();

            var pendingAppointments = dbContext.Appointments
                .Where(a => a.EmployeeId == employeeId && !a.IsApproved)
                .Include(a => a.Operation)
                .ToList();
            return View(pendingAppointments);
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