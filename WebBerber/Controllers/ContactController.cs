using Microsoft.AspNetCore.Mvc;
using WebBerber.Models;
using WebBerber.Utils;

namespace WebBerber.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitContactForm(Contact model)
        {
            if (ModelState.IsValid)
            {
                // Debugging için modelin içeriğini kontrol edelim
                Console.WriteLine($"Name: {model.Name}, Surname: {model.Surname}, Email: {model.Email}, Subject: {model.Subject}, Message: {model.Message}");

                // Form verilerini veritabanına kaydedelim
                _context.ContactForms.Add(model);
                _context.SaveChanges();

                // Başarı mesajını TempData'ya ekliyoruz
                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi. En kısa sürede dönüş yapacağız!";
                return RedirectToAction("Index");
            }

            // Eğer form geçerli değilse, hata mesajlarını ve formu geri döndürüyoruz
            return View("Index", model);
        }

    }
}
