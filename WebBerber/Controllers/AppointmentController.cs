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

            var availability = GetAvailableTimesForDay(employeeId, DateTime.Today, operationDuration);

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            ViewBag.EmployeeId = employeeId;
            return View(availability);
        }

        private IEnumerable<DateTime> GetAvailableTimesForDay(int employeeId, DateTime day, int duration)
        {
            var startHour = day.Date.AddHours(9);
            var endHour = day.Date.AddHours(19);
            var allTimes = new List<DateTime>();

            while (startHour < endHour)
            {
                allTimes.Add(startHour);
                startHour = startHour.AddMinutes(60);
            }

            var existingAppointments = appDbContext.Appointments
                .Where(a => a.EmployeeId == employeeId && a.StartTime.Date == day.Date)
                .Select(a => a.StartTime)
                .ToList();

            return allTimes.Where(t => !existingAppointments.Any(a => t < a.AddMinutes(duration) && a < t.AddMinutes(duration)));
        }

        public IActionResult SelectTime(string selectedDate, int shopId, int operationId, int employeeId)
        {
            if (string.IsNullOrEmpty(selectedDate))
            {
                selectedDate = DateTime.Today.ToString("yyyy-MM-dd");
            }

            DateTime selectedDay = DateTime.Parse(selectedDate);

            int operationDuration = appDbContext.Operations
                .Where(o => o.Id == operationId)
                .Select(o => o.Duration)
                .FirstOrDefault();

            var availableTimes = GetAvailableTimesForDay(employeeId, selectedDay, operationDuration);

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            ViewBag.EmployeeId = employeeId;
            ViewBag.SelectedDate = selectedDate;

            return View("SelectTime", availableTimes);
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

            var appointment = new Appointment
            {
                EmployeeId = employeeId,
                OperationId = operationId,
                StartTime = startTime,
                Duration = operation?.Duration ?? 0,
                Price = operation?.Price ?? 0,
                IsApproved = false
            };

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
                İyi günler dileriz.";

            try
            {
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential("erencsahin34@gmail.com", "jmef uvro bxgi wrjn"),
                    EnableSsl = true
                };

                smtpClient.Send("erencsahin34@gmail.com", email, subject, body);

                appDbContext.Appointments.Add(appointment);
                appDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Randevu isteği başarıyla oluşturuldu ve e-posta gönderildi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"E-posta gönderimi başarısız: {ex.Message}";
            }

            return RedirectToAction("ListShops", "Customer");
        }

        [HttpPost]
        public IActionResult ApproveOrReject(int appointmentId, string action)
        {
            var appointment = appDbContext.Appointments.FirstOrDefault(a => a.Id == appointmentId);

            if (appointment == null)
            {
                TempData["ErrorMessage"] = "Randevu bulunamadı.";
                return RedirectToAction("PendingAppointments", "Employee");
            }

            if (action == "approve")
            {
                appointment.IsApproved = true;
                TempData["SuccessMessage"] = "Randevu başarıyla onaylandı.";
            }
            else if (action == "reject")
            {
                appDbContext.Appointments.Remove(appointment);
                TempData["ErrorMessage"] = "Randevu reddedildi.";
            }

            appDbContext.SaveChanges();
            return RedirectToAction("PendingAppointments", "Employee");
        }
    }
}
