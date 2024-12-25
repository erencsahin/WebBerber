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
                _context.ContactForms.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi. En kısa sürede dönüş yapacağız!";
                return RedirectToAction("Index");
            }
            return View("Index", model);
        }

    }
}
