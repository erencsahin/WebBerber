using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Index(string email, string password)
        {

            var hashedPass=Security.HashPassword(password);


            var user = appDbContext.Users.FirstOrDefault(x => x.Email == email && x.Password == hashedPass);
            var employee=appDbContext.Employees.FirstOrDefault(e=>e.Email==email && e.Password==hashedPass);
            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserRole",user.Role.ToString());

                switch (user.Role)
                {
                    case Role.Admin:
                        return RedirectToAction("Index","Admin");
                    case Role.Customer:
                        return RedirectToAction("Index", "Home");
                }
            }
            if (employee!=null)
            {
                HttpContext.Session.SetString("EmployeeEmail", employee.Email);
                HttpContext.Session.SetInt32("EmployeeId", employee.Id);

                return RedirectToAction("Index", "Home");   
            }

            ViewBag.ErrorMessage = "Email veya şifre yanlış.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
