using Microsoft.EntityFrameworkCore;
using WebBerber.Models;


namespace WebBerber.Utils
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<WorkingHour> WorkHours { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeOperation> EmployeeOperations { get; set; }
        public DbSet<Contact> ContactForms { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmployeeOperation>()
            .HasKey(eo => new { eo.EmployeeId, eo.OperationId });

            modelBuilder.Entity<EmployeeOperation>()
                .HasOne(eo => eo.Employee)
                .WithMany(eo => eo.EmployeeOperations)
                .HasForeignKey(eo => eo.EmployeeId);

            modelBuilder.Entity<EmployeeOperation>()
                .HasOne(eo => eo.Operation)
                .WithMany(eo => eo.EmployeeOperations)
                .HasForeignKey(eo => eo.OperationId);

        }
    }
}
