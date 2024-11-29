using Microsoft.AspNetCore.Mvc;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext appDbContext;
        public LoginController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(string email, string password)
        {

            var hashedPass=Security.HashPassword(password);


            var user= appDbContext.Users.FirstOrDefault(x=>x.Email==email && x.Password== hashedPass);
            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole",user.Role.ToString());

                switch (user.Role)
                {
                    case Role.Admin:
                        return RedirectToAction("Index","Admin");
                    case Role.ShopOwner:
                        return RedirectToAction("PendingAppointments", "ShopOwner");
                    case Role.Customer:
                        return RedirectToAction("ListShops", "Customer");
                }
            }

            ViewBag.ErrorMessage = "Email veya şifre yanlış.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
