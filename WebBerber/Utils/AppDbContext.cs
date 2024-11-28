using Microsoft.EntityFrameworkCore;
using WebBerber.Models;


namespace WebBerber.Utils
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<WorkingHour> WorkHours { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
