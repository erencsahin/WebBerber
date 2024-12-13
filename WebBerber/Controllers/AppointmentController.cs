using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            // Uygun saatleri hesaplama (örnek)
            var availability = CalculateAvailability(appointments, operationDuration);

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            ViewBag.EmployeeId = employeeId;
            return View(availability);
        }

        private List<DateTime> CalculateAvailability(List<Appointment> appointments, int duration)
        {
            // Sabah 9'dan akşam 5'e kadar kontrol
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

                start = start.AddMinutes(30); // Yarım saatlik dilimler
            }

            return availableTimes;
        }

        public IActionResult Summary(int employeeId, int operationId, DateTime startTime)
        {
            // Çalışanı ve bağlı olduğu mağazayı getir
            var employee = appDbContext.Employees
                .Include(e => e.Shop) // Çalışanın mağaza bilgilerini de dahil et
                .FirstOrDefault(e => e.Id == employeeId);

            // İşlem bilgilerini getir
            var operation = appDbContext.Operations.FirstOrDefault(o => o.Id == operationId);

            // ViewBag ile detayları View'e gönder
            ViewBag.ShopName = employee?.Shop?.ShopName;
            ViewBag.EmployeeName = employee?.Name;
            ViewBag.OperationName = operation?.OperationName;
            ViewBag.OperationPrice = operation?.Price;

            // Randevu nesnesini View'e gönder
            var appointment = new Appointment
            {
                EmployeeId = employeeId,
                OperationId = operationId,
                StartTime = startTime
            };

            return View(appointment); // Summary.cshtml dosyasına gönder
        }

        [HttpPost]
        public IActionResult SendEmail(string email, int employeeId, int operationId, DateTime startTime)
        {
            // Çalışanı ve bağlı olduğu mağazayı getir
            var employee = appDbContext.Employees
                .Include(e => e.Shop) // Çalışanın mağaza bilgilerini de dahil et
                .FirstOrDefault(e => e.Id == employeeId);

            // İşlem bilgilerini getir
            var operation = appDbContext.Operations.FirstOrDefault(o => o.Id == operationId);

            // E-posta içeriğini oluştur
            string subject = "Randevu Onayı";
            string body = $@"
        Merhaba,
        Randevu detaylarınız aşağıdadır:
        - Mağaza: {employee?.Shop?.ShopName}
        - Çalışan: {employee?.Name}
        - İşlem: {operation?.OperationName}
        - Tarih ve Saat: {startTime:dd MMM yyyy HH:mm}
        - Fiyat: {operation?.Price} ₺

        İyi günler dileriz.
    ";

            try
            {
                // Mail gönderim işlemi
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.example.com")
                {
                    Port = 587,
                    Credentials = new System.Net.NetworkCredential("your_email@example.com", "your_password"),
                    EnableSsl = true,
                };

                smtpClient.Send("your_email@example.com", email, subject, body);

                TempData["SuccessMessage"] = "Randevu özeti e-posta ile gönderildi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"E-posta gönderimi başarısız: {ex.Message}";
            }

            return RedirectToAction("ListShops");
        }



    }
}
