using Microsoft.AspNetCore.Mvc;
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
            return View(); 
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
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
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
        
        public IActionResult DeleteShop(int shopId)
        {
            var shop=dbContext.Shops.Find(shopId);
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
                employee.WorkingHours = filteredWorkingHours;
                dbContext.Employees.Add(employee);
                dbContext.SaveChanges();
                return RedirectToAction("ManageEmployees");
            }

            ViewBag.Shops = dbContext.Shops.ToList();
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


        public IActionResult Logout()
        {
            return RedirectToAction("Index","Login");
        }
    }
}
