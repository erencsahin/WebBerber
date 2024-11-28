using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Dükkan ismi zorunludur.")]
        public string ShopName { get; set; }

        [Required(ErrorMessage ="Çalışan sayınızı giriniz.")]
        [Range(1, int.MaxValue, ErrorMessage = "Çalışan sayısı en az 1 olmalıdır.")]
        public int Employees { get; set; }


        [Required(ErrorMessage ="Dükkanın adresini giriniz.")]
        public string Address { get; set; }


        [Required(ErrorMessage ="Çalışma saatlerini giriniz.")]
        public ICollection<WorkingHour> WorkingHours { get; set; }

        public ICollection<Employee> EmployeesList { get; set; }

    }
}
