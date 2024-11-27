using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required(ErrorMessage = "Çalışma saatlerini giriniz.")]
        public ICollection<WorkingHour> WorkingHours { get; set; }

        public ICollection<Operation> Abilities { get; set; }


        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}
