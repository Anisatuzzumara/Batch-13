using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntyFrame.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Author { get; set; }

        [Required]
        public string? Genre { get; set; }

        [Required]
        public string? Title { get; set; }

        [Column(TypeName = "decimal(11,2)")]
        public decimal Price { get; set; }

    }
}


