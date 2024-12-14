using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
            int employeeId = GetLoggedInEmployeeId();

            var pendingappointments = dbContext.Appointments
                .Where(a => a.EmployeeId == employeeId && !a.IsApproved)
                .Include(a => a.Operation)
                .ToList();
            return View(pendingappointments);
        }


        [HttpPost]
        public IActionResult ApproveOrReject(int appointmentId, string action)
        {
            var appointment= dbContext.Appointments .FirstOrDefault(a=>a.Id == appointmentId);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Randevu bulunamadı.";
                return RedirectToAction("PendingAppointments");
            }

            if (action == "approve")
            {
                appointment.IsApproved = true;
                TempData["SuccessMessage"] = "Randevu onaylandı.";
            }
            else if (action == "reject")
            {
                dbContext.Appointments.Remove(appointment);
                TempData["ErrorMessage"] = "Randevu reddedildi.";
            }

            dbContext.SaveChanges();
            return RedirectToAction("PendingAppointments");
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