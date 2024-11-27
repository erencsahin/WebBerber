using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class WorkingHour
    {
        public int Id { get; set; }

        [Required]
        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

    }
}