using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntyFrame.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderPlaced { get; set; }

        public DateTime? OrderFulfilled { get; set; }

        // foreign key relationship to Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        // navigation property (one-to-many relationship)
        public ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
