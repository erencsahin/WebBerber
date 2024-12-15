using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly AppDbContext appDbContext;

        public AppointmentController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult CheckAvailability(int employeeId, int shopId, int operationId)
        {
            var operationDuration = appDbContext.Operations
                .Where(op => op.Id == operationId)
                .Select(op => op.Duration)
                .FirstOrDefault();

            var appointments = appDbContext.Appointments
                .Where(a => a.EmployeeId == employeeId)
                .ToList();

            var availability = CalculateAvailability(appointments, operationDuration);

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            ViewBag.EmployeeId = employeeId;
            return View(availability);
        }

        private List<DateTime> CalculateAvailability(List<Appointment> appointments, int duration)
        {
            var availableTimes = new List<DateTime>();
            DateTime start = DateTime.Today.AddHours(9);
            DateTime end = DateTime.Today.AddHours(17);

            while (start < end)
            {
                bool isConflict = appointments.Any(a =>
                    a.StartTime < start.AddMinutes(duration) &&
                    a.StartTime.AddMinutes(a.Duration) > start);

                if (!isConflict)
                {
                    availableTimes.Add(start);
                }

                start = start.AddMinutes(30);
            }

            return availableTimes;
        }

        public IActionResult Summary(int employeeId, int operationId, DateTime startTime)
        {
            var employee = appDbContext.Employees
                .Include(e => e.Shop)
                .FirstOrDefault(e => e.Id == employeeId);

            var operation = appDbContext.Operations.FirstOrDefault(o => o.Id == operationId);

            ViewBag.ShopName = employee?.Shop?.ShopName;
            ViewBag.EmployeeName = employee?.Name;
            ViewBag.OperationName = operation?.OperationName;
            ViewBag.OperationPrice = operation?.Price;

            var appointment = new Appointment
            {
                EmployeeId = employeeId,
                OperationId = operationId,
                StartTime = startTime
            };

            return View(appointment);
        }

        [HttpPost]
        public IActionResult SendEmail(string email, int employeeId, int operationId, DateTime startTime)
        {
            var employee = appDbContext.Employees
                .Include(e => e.Shop)
                .FirstOrDefault(e => e.Id == employeeId);

            var operation = appDbContext.Operations.FirstOrDefault(o => o.Id == operationId);

            string subject = "Randevu Onayı";
            string body = $@"
        Merhaba,
        Randevu detaylarınız aşağıdadır:
        - Mağaza: {employee?.Shop?.ShopName}
        - Çalışan: {employee?.Name}
        - İşlem: {operation?.OperationName}
        - Tarih ve Saat: {startTime:dd MMM yyyy HH:mm}
        - Fiyat: {operation?.Price} ₺
        - Adres: {employee?.Shop?.Address}

        İyi günler dileriz.
    ";

            try
            {
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential("erencsahin34@gmail.com", "jmef uvro bxgi wrjn"),
                    EnableSsl = true
                };

                string fromAddress = "erencsahin34@gmail.com"; 
                string toAddress = email;
                string mailSubject = subject;
                string mailBody = body;

                smtpClient.Send(fromAddress, toAddress, mailSubject, mailBody);
                

                //var operation = appDbContext.Operations.FirstOrDefault(o => o.Id == operationId);
                var appointment = new Appointment
                {
                    EmployeeId = employeeId,
                    OperationId = operationId,
                    StartTime = startTime,
                    Duration = operation?.Duration ?? 0,
                    Price = operation?.Price ?? 0,
                    IsApproved = false
                };

                appDbContext.Appointments.Add(appointment);
                Console.WriteLine("**************************************DBYE KAYDEDİLDİ**************************************");
                appDbContext.SaveChanges();

                Console.WriteLine("E-posta gönderildi.");
                TempData["SuccessMessage"] = "Randevu isteği başarıyla oluşturuldu ve e-posta gönderildi.";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"E-posta gönderimi başarısız: {ex.Message}";
                Console.WriteLine($"E-posta gönderimi hatası: {ex.Message}");
            }
            return RedirectToAction("ListShops","Customer");
        }


        [HttpPost]
        public IActionResult ApproveOrReject(int appointmentId, string action)
        {
            var appointment = appDbContext.Appointments.FirstOrDefault(a => a.Id == appointmentId);

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
                appDbContext.Appointments.Remove(appointment);
                TempData["ErrorMessage"] = "Randevu reddedildi.";
            }

            appDbContext.SaveChanges();
            return RedirectToAction("PendingAppointments","Employee");
        }
    }
}
