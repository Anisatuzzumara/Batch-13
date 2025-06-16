using System;
using System.ComponentModel.DataAnnotations;

namespace BookStoreMVC.Models;

public class Book
{

    [Required(ErrorMessage = "Book ID cannot empty")]
    public int BookID { get; set; }
    public int AuthorID { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title name cannot more than 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0, 100, ErrorMessage = "Price must be between 0 and 50")]
    [Display(Name = "Grade (%)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Date Recorde")]
    public DateTime ReleaseDate { get; set; }
        
    // Properti navigasi untuk mengakses data penulis terkait
    public virtual Author? Author { get; set; }
}
