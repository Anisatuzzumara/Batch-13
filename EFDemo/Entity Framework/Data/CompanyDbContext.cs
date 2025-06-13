using Entity_Framework.Models;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework.Data
{
    public class CompanyDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options) { }
        public CompanyDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=CompanyDatabase.db");
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasOne(e => e.Department)
                      .WithMany(d => d.Employees)
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasIndex(d => d.Name).IsUnique();
                entity.HasOne(d => d.Manager)
                      .WithMany()
                      .HasForeignKey(d => d.ManagerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configure many-to-many relationship
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Projects)
                .WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeProject",
                    j => j.HasOne<Project>().WithMany().HasForeignKey("ProjectId"),
                    j => j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                    j =>
                    {
                        j.HasKey("EmployeeId", "ProjectId");
                        j.ToTable("EmployeeProjects");
                    });
        }
    }
}