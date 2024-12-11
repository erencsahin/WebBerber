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
            Console.WriteLine("Shops Count: " + shops.Count); // Mağaza sayısını logla
            return View(shops);
        }

        public IActionResult SelectOperation(int shopId)
        {
            var operations = appDbContext.Operations
                .Where(op => appDbContext.EmployeeOperations
                    .Any(eo => eo.OperationId == op.Id &&
                               appDbContext.Employees.Any(e => e.Id == eo.EmployeeId && e.ShopId == shopId)))
                .ToList();

            ViewBag.ShopId = shopId; // shopId'yi ViewBag ile gönderiyoruz
            return View(operations); // IEnumerable<Operation> modeli gönderiliyor
        }




        public IActionResult SelectEmployee(int shopId, int operationId)
        {
            var employees = appDbContext.Employees
                .Where(e => e.ShopId == shopId &&
                            appDbContext.EmployeeOperations.Any(eo => eo.EmployeeId == e.Id && eo.OperationId == operationId))
                .ToList();

            ViewBag.ShopId = shopId;
            ViewBag.OperationId = operationId;
            return View(employees);
        }



        public IActionResult CheckAvailability(int employeeId, int operationId)
        {
            var operationDuration = appDbContext.Operations
                .Where(op => op.Id == operationId)
                .Select(op => op.Duration)
                .FirstOrDefault();

            var appointments = appDbContext.Appointments
                .Where(a => a.EmployeeId == employeeId)
                .ToList();

            // Uygun saatleri hesaplamak
            var availability = CalculateAvailability(appointments, operationDuration);

            ViewBag.EmployeeId = employeeId;
            ViewBag.OperationId = operationId;
            return View(availability);
        }

        private List<DateTime> CalculateAvailability(List<Appointment> appointments, int duration)
        {
            // Burada mevcut randevuları kontrol ederek uygun saatleri hesaplayabilirsiniz.
            // Örnek olarak sabah 9'dan akşam 5'e kadar olan saatleri hesaplıyoruz:
            List<DateTime> availableTimes = new List<DateTime>();
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

                start = start.AddMinutes(30); // Her yarım saatte bir kontrol
            }

            return availableTimes;
        }


    }
}
