using Microsoft.AspNetCore.Mvc;
using WebBerber.Filters;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
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




        public IActionResult ManageShops()
        {
            var shops = dbContext.Shops.ToList();
            return View(shops);
        }

        public IActionResult AddShop()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddShop(Shop shop,List<WorkingHour> workingHours)
        {
            if (ModelState.IsValid)
            {
                dbContext.Shops.Add(shop);
                dbContext.SaveChanges();

                foreach (var hour in workingHours)
                {
                    hour.ShopId = shop.Id;
                    dbContext.WorkHours.Add(hour);
                }
                dbContext.SaveChanges();

                return RedirectToAction("ManageShops");
            }
            return View(shop);
        }

        public IActionResult DeleteShop(int id)
        {
            var shop = dbContext.Shops.Find(id);
            if (shop != null)
            {
                dbContext.Shops.Remove(shop);
                dbContext.SaveChanges();
            }
            return RedirectToAction("ManageShops");
        }

    }
}
