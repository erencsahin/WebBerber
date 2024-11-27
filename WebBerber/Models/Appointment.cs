namespace WebBerber.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int EmployeeId { get; set; }

        public int OperationId { get; set; }

        public DateTime Date { get; set; }
    }
}
