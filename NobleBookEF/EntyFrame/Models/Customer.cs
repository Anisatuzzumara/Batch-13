using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntyFrame.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        // navigation property (one-to-many relationship)
        public ICollection<Order> Orders { get; set; } = null!;
        
    }

}

