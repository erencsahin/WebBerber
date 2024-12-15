using System.ComponentModel.DataAnnotations;

namespace WebBerber.Models
{
    public class PendingAppointments
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int OperationId { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsApproved { get; set; }
    }
}
