using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity_Framework.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }
        
        public DateTime HireDate { get; set; }
        
        [MaxLength(100)]
        public string? Position { get; set; }
        
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; } = null!;
        
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        
        public bool IsActive { get; set; } = true;
    }
}