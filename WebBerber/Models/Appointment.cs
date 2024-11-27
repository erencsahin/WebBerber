using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBerber.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } // Navigasyon özelliği

        [Required]
        public int OperationId { get; set; }
        public Operation Operation { get; set; } // Navigasyon özelliği

        [Required]
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "Randevu tarihi gelecekte bir tarih olmalıdır.")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public int Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir süre giriniz.")]
        public int Duration { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime > DateTime.Now;
            }
            return false;
        }
    }
}
