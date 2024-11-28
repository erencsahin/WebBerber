using Microsoft.AspNetCore.Mvc;
using WebBerber.Filters;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    public class AdminController : Controller
    {
        public readonly AppDbContext dbContext;
        public AdminController(AppDbContext appDbContext)
        {
            this.dbContext = appDbContext;
        }

        [AdminAuthorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [AdminAuthorize]
        public IActionResult ManageUsers()
        {
            var _users=dbContext.Users.ToList();
            return View(_users);
        }

        [AdminAuthorize]
        public IActionResult ManageShops()
        {
            var _shops=dbContext.Shops.ToList();
            return View(_shops);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminEmail");
            return RedirectToAction("Index","Login");
        }
    }
}
