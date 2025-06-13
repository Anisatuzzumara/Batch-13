using System.ComponentModel.DataAnnotations;

namespace Entity_Framework.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public decimal Budget { get; set; }
        
        [MaxLength(50)]
        public string Status { get; set; } = "Active";
        
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
