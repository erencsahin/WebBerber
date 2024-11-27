using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class Operation
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="İşlemin adını giriniz.")]
        public int OperationName { get; set; }
        public int Price { get; set; }
        public int Duration { get; set; }
    }
}