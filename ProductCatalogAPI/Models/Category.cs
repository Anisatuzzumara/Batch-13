using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models
{
    /// <summary>
    /// Represents a product category in our system.
    /// This is a domain entity that maps directly to the database table.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation property: One category can have many products
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}