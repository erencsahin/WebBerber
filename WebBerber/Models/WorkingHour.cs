using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class WorkingHour
    {
        public int Id { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required(ErrorMessage ="Lütfen bir saat giriniz.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Lütfen bir saat giriniz.")]
        public TimeSpan EndTime { get; set; }

        public int? ShopId { get; set; }
        public Shop Shop { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}