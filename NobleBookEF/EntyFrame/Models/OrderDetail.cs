
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntyFrame.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int BookId { get; set; }

        public int OrderId { get; set; }

        // foreign key relationship to Order and Book
        public Order? Order { get; set; }
        public Book? Book { get; set; }
    }
}
    



