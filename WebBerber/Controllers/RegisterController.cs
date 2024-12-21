using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {

        private readonly AppDbContext appDbContext;

        public RegisterController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            model.Role = Role.Customer;

            if (appDbContext.Users.Any(u => u.Email == model.Email)) 
            {
                ViewBag.ErrorMessage = "Bu email ile zaten kayıt var.";
                return View(model);
            
            }
            model.Password=Security.HashPassword(model.Password);
            appDbContext.Users.Add(model);
            appDbContext.SaveChanges();


            TempData["SuccessMessage"] ="Kayıt başarılı. Giriş sayfasına yönlendiriliyorsunuz.";
            return RedirectToAction("Index","Login");
        }
    }
}
