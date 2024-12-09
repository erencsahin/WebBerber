using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<WorkingHour>? WorkingHours { get; set; } = new List<WorkingHour>();
        public ICollection<Operation>? Operations { get; set; }= new List<Operation>();
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}
