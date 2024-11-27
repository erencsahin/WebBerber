using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [Required(ErrorMessage = "Çalışma saatlerini giriniz.")]
        public ICollection<WorkingHour> WorkingHours { get; set; }

        public ICollection<Operation> Abilities { get; set; }

    }
}
