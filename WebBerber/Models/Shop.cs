using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Dükkan ismi zorunludur.")]
        public string ShopName { get; set; }


        [Required(ErrorMessage ="Dükkanın adresini giriniz.")]
        public string Address { get; set; }

        public ICollection<Employee>? EmployeesList { get; set; }
    }
}
