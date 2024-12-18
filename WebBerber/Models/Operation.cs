using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Operation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="İşlemin adını giriniz.")]
        public string OperationName { get; set; }
        public int Price { get; set; }
        public int Duration { get; set; }
        public string Gender { get; set; }
        public ICollection<EmployeeOperation> EmployeeOperations { get; set; }
    }
}


/*
 
    HER DUKKANDA FIYATLAR FARKLI OLABILIR.
 
 */