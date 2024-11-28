using Microsoft.AspNetCore.Mvc;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
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

            if (model.Role==Role.Admin) 
            {
                ViewBag.ErrorMessage = "Admin olarak kayıt yapılamaz.";
                return View();
            }

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
