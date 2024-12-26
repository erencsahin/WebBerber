using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebBerber.Models;
using WebBerber.Utils;
using System.Net.Mail;

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
                .Where(a => a.EmployeeId == employeeId && a.StartTime.Date == DateTime.Today.Date)
                .ToList();

            var availableTimes = GetAvailableTimesForDay(employeeId, DateTime.Today, operationDuration);

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            ViewBag.EmployeeId = employeeId;

            return View(availableTimes);
        }

        private IEnumerable<DateTime> GetAvailableTimesForDay(int employeeId, DateTime day, int duration)
        {
            var startHour = day.Date.AddHours(9);
            var endHour = day.Date.AddHours(19);

            // Mevcut randevuları çek
            var existingAppointments = appDbContext.Appointments
                .Where(a => a.EmployeeId == employeeId && a.StartTime.Date == day.Date)
                .Select(a => new
                {
                    Start = a.StartTime,
                    End = a.StartTime.AddMinutes(a.Duration)
                })
                .ToList();

            var availableTimes = new List<DateTime>();

            while (startHour.AddMinutes(duration) <= endHour)
            {
                bool isConflict = existingAppointments.Any(a =>
                    // Yeni randevunun başlangıcı mevcut bir randevunun içinde mi?
                    (startHour >= a.Start && startHour < a.End) ||
                    // Yeni randevunun bitişi mevcut bir randevunun içinde mi?
                    (startHour.AddMinutes(duration) > a.Start && startHour.AddMinutes(duration) <= a.End) ||
                    // Yeni randevu tamamen mevcut bir randevunun üstüne mi denk geliyor?
                    (startHour <= a.Start && startHour.AddMinutes(duration) >= a.End));

                // Çakışma yoksa, zamanı uygun olarak ekle
                if (!isConflict)
                {
                    availableTimes.Add(startHour);
                }

                // Bir sonraki kontrol için saat ilerlet
                startHour = startHour.AddMinutes(30); // Her 30 dakikada bir kontrol
            }

            return availableTimes;
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

            var appointments = appDbContext.Appointments
                .Where(a => a.EmployeeId == employeeId && a.StartTime.Date == selectedDay.Date)
                .ToList();

            var availableTimes = GetAvailableTimesForDay(employeeId, selectedDay, operationDuration);

            var now = DateTime.Now;
            availableTimes = availableTimes.Where(time => time > now).ToList();

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            ViewBag.EmployeeId = employeeId;
            ViewBag.SelectedDate = selectedDate;

            return View(availableTimes);
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
        public IActionResult SendEmail(int employeeId, int operationId, DateTime startTime)
        {
            var employee = appDbContext.Employees
                .Include(e => e.Shop)
                .FirstOrDefault(e => e.Id == employeeId);

            var operation = appDbContext.Operations.FirstOrDefault(o => o.Id == operationId);

            var customerEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(customerEmail) || employee == null || operation == null)
            {
                TempData["ErrorMessage"] = "Randevu oluşturulurken bit hata oluştu.";
                return RedirectToAction("ListShops", "Customer");
            }


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

                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("erencsahin34@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(customerEmail);

                if (!string.IsNullOrEmpty(employee.Email))
                {
                    mailMessage.To.Add(employee.Email);
                }

                smtpClient.Send(mailMessage);

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