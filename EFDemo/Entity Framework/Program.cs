using Entity_Framework.Data;
using Entity_Framework.Services;
using Microsoft.EntityFrameworkCore;
using Entity_Framework.Models; // Add this if Department is in the Models namespace

namespace Entity_Framework
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Entity Framework Core Demo ===");

            using var context = new CompanyDbContext();

            // Apply migrations
            await context.Database.MigrateAsync();

            // Seed data
            await SeedDatabaseAsync(context);


            // Initialize services
            var employeeService = new EmployeeService(context);

            // Demo CRUD operations
            await DemonstrateCrudOperationsAsync(employeeService);
        }


        static async Task DemonstrateCrudOperationsAsync(EmployeeService service)
        {
            
        }


        static async Task SeedDatabaseAsync(CompanyDbContext context)
        {
            if (await context.Departments.AnyAsync()) return;

            var departments = new[]
            {
                new Department { Name = "Engineering", Description = "Software development", Budget = 500000m },
                new Department { Name = "Sales", Description = "Revenue generation", Budget = 300000m },
                new Department { Name = "Marketing", Description = "Social Media Specicialist", Budget = 300000m},
                new Department { Name = "IT Support", Description = "Technical Support", Budget = 2700000m}
            };

            context.Departments.AddRange(departments);
            await context.SaveChangesAsync();

            // Add employees and projects similarly...
        }
    }
}
