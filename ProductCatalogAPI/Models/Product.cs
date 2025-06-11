using System.ComponentModel.DataAnnotations;

namespace ProductCatalogAPI.Models
{
    /// <summary>
    /// Represents a product in our catalog.
    /// Contains validation rules and business logic for products.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int StockQuantity { get; set; }

        // Foreign key to Category
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Navigation property: Many products belong to one category
        public virtual Category Category { get; set; } = null!;

        // Business logic methods
        public bool IsInStock() => StockQuantity > 0;
        public bool IsLowStock(int threshold = 10) => StockQuantity <= threshold && StockQuantity > 0;
    }
}