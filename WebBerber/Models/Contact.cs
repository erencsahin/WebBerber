using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Contact
    {

        [Key] 
        public int Id { get; set; } 

        [Required(ErrorMessage = "Adınız zorunludur.")]
        [MaxLength(50, ErrorMessage = "Adınız 50 karakteri geçemez.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyadınız zorunludur.")]
        [MaxLength(50, ErrorMessage = "Soyadınız 50 karakteri geçemez.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "E-posta adresiniz zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Konu zorunludur.")]
        [MaxLength(100, ErrorMessage = "Konu 100 karakteri geçemez.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Mesajınız zorunludur.")]
        [MaxLength(1000, ErrorMessage = "Mesajınız 1000 karakteri geçemez.")]
        public string Message { get; set; }
    }
}
