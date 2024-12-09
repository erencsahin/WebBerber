using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class WorkingHour
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        [Required(ErrorMessage ="Lütfen bir saat giriniz.")]
        public TimeSpan StartTime { get; set; }


        [Required(ErrorMessage = "Lütfen bir saat giriniz.")]
        public TimeSpan EndTime { get; set; }
    }
}