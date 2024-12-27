using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebBerber.Filters;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        public readonly AppDbContext dbContext;
        public AdminController(AppDbContext appDbContext)
        {
            dbContext = appDbContext;
        }

        public IActionResult Index()
        {
            var shops = dbContext.Shops
                .Include(s => s.EmployeesList)
                .ToList();

            var shopStatistics = shops.Select(shop => new
            {
                ShopId = shop.Id,
                ShopName = shop.ShopName,
                TotalEmployees = shop.EmployeesList.Count,
                TotalOperations = shop.EmployeesList
                    .SelectMany(e => dbContext.Appointments
                        .Where(a => a.EmployeeId == e.Id && a.IsApproved))
                    .Count(),
                TotalEarnings = shop.EmployeesList
                    .SelectMany(e => dbContext.Appointments
                        .Where(a => a.EmployeeId == e.Id && a.IsApproved))
                    .Sum(a => a.Price)
            }).ToList();

            return View(shopStatistics);
        }



        public IActionResult ManageUsers()
        {
            var users = dbContext.Users.ToList();
            return View(users);
        }

        public IActionResult ManageShops()
        {
            var shops = dbContext.Shops.ToList();
            return View(shops);
        }
        
        public IActionResult ManageEmployees()
        {
            var employees = dbContext.Employees
                .Include(e => e.WorkingHours)
                .Include(e => e.Shop)
                .ToList();
            return View(employees);
        }

        public IActionResult ManageShopOwners()
        {
            var owners = dbContext.Users
                .Where(u => u.Role == Role.ShopOwner)
                .ToList();
            return View(owners);
        }


        public IActionResult AddUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddUser(User user) 
        {
            if (ModelState.IsValid) 
            {
                user.Password=Security.HashPassword(user.Password);
                
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
                return RedirectToAction("ManageUsers");
            }
            TempData["ErrorMessage"] = "Kullanıcı eklenirken bir hata oluştu.";
            return View(user);
        }

        public IActionResult EditUser(int id)
        {
            var user = dbContext.Users.Find(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("ManageUsers");
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = dbContext.Users.Find(user.Id);
                if (existingUser == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                    return RedirectToAction("ManageUsers");
                }

                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;

                dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction("ManageUsers");
            }

            return View(user);
        }


        public IActionResult DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);
            if (user != null) 
            {
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ManageUsers");
        }

        public IActionResult AddShop()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddShop(Shop shop)
        {
            if (ModelState.IsValid)
            {
                dbContext.Shops.Add(shop);
                dbContext.SaveChanges();
                return RedirectToAction("ManageShops");
            }
            return View(shop);
        }
        public IActionResult EditShop(int shopId)
        {
            var shop = dbContext.Shops.Find(shopId);
            if (shop == null)
            {
                TempData["ErrorMessage"] = "Dükkan bulunamadı.";
                return RedirectToAction("ManageShops");
            }

            return View(shop);
        }

        [HttpPost]
        public IActionResult EditShop(Shop shop)
        {
            if (ModelState.IsValid)
            {
                var existingShop = dbContext.Shops.Find(shop.Id);
                if (existingShop == null)
                {
                    TempData["ErrorMessage"] = "Dükkan bulunamadı.";
                    return RedirectToAction("ManageShops");
                }

                // ShopName özelliğini kullanıyoruz
                existingShop.ShopName = shop.ShopName;
                existingShop.Address = shop.Address;

                dbContext.SaveChanges();

                TempData["SuccessMessage"] = "Dükkan başarıyla güncellendi.";
                return RedirectToAction("ManageShops");
            }

            return View(shop);
        }


        public IActionResult DeleteShop(int shopId)
        {
            var shop = dbContext.Shops.Find(shopId);
            if (shop != null)
            {
                dbContext.Shops.Remove(shop);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ManageShops");
        }

        
        public IActionResult AddEmployee()
        {
            ViewBag.Shops = dbContext.Shops.ToList();
            return View();
        }


        [HttpPost]
        public IActionResult AddEmployee(Employee employee, List<WorkingHour> workingHours)
        {

            var filteredWorkingHours = workingHours.Where(wh => wh.StartTime != TimeSpan.Zero && wh.EndTime != TimeSpan.Zero).ToList();

            if (ModelState.IsValid)
            {
                employee.Password=Security.HashPassword(employee.Password);
                employee.WorkingHours = filteredWorkingHours;
                dbContext.Employees.Add(employee);
                dbContext.SaveChanges();
                return RedirectToAction("ManageEmployees");
            }

            ViewBag.Shops = dbContext.Shops.ToList();
            return View(employee);
        }
        //HATA VAR DÜZENLENECEK 
        public IActionResult EditEmployee(int id)
        {
            var employee = dbContext.Employees
                .Include(e => e.WorkingHours)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                TempData["ErrorMessage"] = "Çalışan bulunamadı.";
                return RedirectToAction("ManageEmployees");
            }

            return View(employee);
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee employee, List<WorkingHour> workingHours)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = dbContext.Employees
                    .Include(e => e.WorkingHours)
                    .Include(e => e.Shop)
                    .FirstOrDefault(e => e.Id == employee.Id);

                if (existingEmployee == null)
                {
                    TempData["ErrorMessage"] = "Çalışan bulunamadı.";
                    return RedirectToAction("ManageEmployees");
                }

                // Çalışan bilgilerini güncelle
                existingEmployee.Name = employee.Name;
                existingEmployee.Surname = employee.Surname;
                existingEmployee.Email = employee.Email;
                existingEmployee.Password= employee.Password;

                // Çalışma saatlerini güncelle
                /*existingEmployee.WorkingHours.Clear();
                foreach (var workingHour in workingHours)
                {
                    existingEmployee.WorkingHours.Add(workingHour);
                }*/

                try
                {
                    dbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Çalışan başarıyla güncellendi.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"SaveChanges hatası: {ex.Message}");
                    TempData["ErrorMessage"] = "Güncelleme sırasında bir hata oluştu.";
                }

                return RedirectToAction("ManageEmployees");
            }

            // ModelState geçerli değilse
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            return View(employee);
        }



        public IActionResult DeleteEmployee(int id)
        {
            var employee=dbContext.Employees
                .Include(s=>s.WorkingHours)
                .FirstOrDefault(s=>s.Id == id);

            if (employee!=null)
            {
                dbContext.WorkHours.RemoveRange(employee.WorkingHours);
                dbContext.Employees.Remove(employee);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ManageEmployee");
        }

        public IActionResult GetShopsStatistics()
        {
            var shops = dbContext.Shops
                .Include(s => s.EmployeesList)
                .ToList();

            var shopStatistics = shops.Select(shop => new
            {
                ShopId = shop.Id,
                ShopName = shop.ShopName,
                TotalEmployees = shop.EmployeesList.Count,
                TotalOperations = shop.EmployeesList
                    .SelectMany(e => dbContext.Appointments
                        .Where(a => a.EmployeeId == e.Id && a.IsApproved))
                    .Count(),
                TotalEarnings = shop.EmployeesList
                    .SelectMany(e => dbContext.Appointments
                        .Where(a => a.EmployeeId == e.Id && a.IsApproved))
                    .Sum(a => a.Price)
            }).ToList();

            return View(shopStatistics);
        }

        public IActionResult GetEmployeeStatistics(int shopId)
        {
            var employees = dbContext.Employees
                .Where(e => e.ShopId == shopId)
                .ToList();

            var employeeStatistics = employees.Select(employee => new
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.Name} {employee.Surname}",
                TotalOperations = dbContext.Appointments
                    .Where(a => a.EmployeeId == employee.Id && a.IsApproved)
                    .Count(),
                TotalEarnings = dbContext.Appointments
                    .Where(a => a.EmployeeId == employee.Id && a.IsApproved)
                    .Sum(a => a.Price),
                TotalDuration = dbContext.Appointments
                    .Where(a => a.EmployeeId == employee.Id && a.IsApproved)
                    .Sum(a => a.Duration),
                OperationDetails = dbContext.Appointments
                    .Where(a => a.EmployeeId == employee.Id && a.IsApproved)
                    .GroupBy(a => new { a.OperationId, a.StartTime.Date })
                    .Select(g => new
                    {
                        OperationName = g.First().Operation.OperationName,
                        Earnings = g.Sum(a => a.Price),
                        TotalDuration = g.Sum(a => a.Duration),
                        Date = g.Key.Date
                    }).ToList()
            }).ToList();

            ViewBag.ShopName = dbContext.Shops.FirstOrDefault(s => s.Id == shopId)?.ShopName;
            return View(employeeStatistics);
        }



        public IActionResult Logout()
        {
            return RedirectToAction("Index","Login");
        }


    }
}
