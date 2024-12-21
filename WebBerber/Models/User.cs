using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Adınızı Giriniz.")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Soyadınızı Giriniz.")]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Telefon numaranızı giriniz.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefon numaranız 11 basamaklı olmalıdır.")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage ="Lütfen geçerli bir mail adresi giriniz.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required(ErrorMessage ="Size daha iyi yardımcı olmak adına lütfen cinsiyetinizi giriniz.")]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }


        [EnumDataType(typeof(Role))]
        [DefaultValue(Role.Customer)]
        public Role Role { get; set; }
    }
}

public enum Gender
{
    Male=1,
    Female=2,
    Other=3
}

public enum Role
{
    Admin=1,
    ShopOwner=2,
    Customer=3,
    Employee=4
}