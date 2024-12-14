using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }


        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


        [Required]
        public int OperationId { get; set; }
        public Operation Operation { get; set; }


        [Required]
        public DateTime StartTime { get; set; }

        public DateTime EndTime => StartTime.AddMinutes(Duration);


        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Geçerli bir fiyat giriniz.")]
        public int Price { get; set; }


        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir süre giriniz.")]
        public int Duration { get; set; }

        public bool IsApproved { get; set; } = false;
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
