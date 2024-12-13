using Microsoft.AspNetCore.Mvc;
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

        public IActionResult PendingAppointments()
        {
            var appointments = dbContext.Appointments
                .Where(a => !a.IsApproved)
                .ToList();
            return View(appointments);
        }


        [HttpPost]
        public IActionResult ApproveAppointment(int id)
        {
            var appointment = dbContext.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.IsApproved = true;
                dbContext.SaveChanges();
            }

            return RedirectToAction("PendingAppointments");
        }
    }
}
