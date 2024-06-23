using Microsoft.EntityFrameworkCore;

namespace CRMsystem.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Employee> employees { get; set; } = null!;
        public DbSet<Task> tasks { set; get; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasOne(t => t.Employee)       
                .WithMany(e => e.Tasks)      
                .HasForeignKey(t => t.EmployeeId); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
