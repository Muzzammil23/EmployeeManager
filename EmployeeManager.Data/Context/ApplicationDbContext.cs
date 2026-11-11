using EmployeeManager.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasIndex(employee =>
                    employee.EmployeeNumber)
                .IsUnique();
        }

        public DbSet<Employee> Employees { get; set; }
    }
}