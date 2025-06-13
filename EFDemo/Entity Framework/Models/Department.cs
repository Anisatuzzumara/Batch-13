using System.ComponentModel.DataAnnotations;

namespace Entity_Framework.Models
{
    public class Department
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public decimal Budget { get; set; }
        
        public int? ManagerId { get; set; }
        public virtual Employee? Manager { get; set; }
        
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
