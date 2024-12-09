using Microsoft.AspNetCore.Mvc;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    public class ShopOwnerController : Controller
    {

        private readonly AppDbContext dbContext;

        public ShopOwnerController(AppDbContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult AddOperationToEmployee(int employeeId,int operationId)
        {
            
            
            return View();
        }


    }
}
