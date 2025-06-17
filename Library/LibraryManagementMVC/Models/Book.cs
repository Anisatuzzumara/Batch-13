using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementMVC.Models;

public class Book
{
    [Required(ErrorMessage = "PubId cannot empty")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "BookId cannot empty")]
    public int PubId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title name cannot more than 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author cannot empty")]
    [StringLength(150, ErrorMessage = "Author cannot more than 150 characters")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "ISBN cannot empty")]
    [StringLength(150, ErrorMessage = "ISBN cannot more than 150 characters")]
    public string ISBN { get; set; } = string.Empty;

    [Required(ErrorMessage = "Publish Year is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Year Published")]
    public int PublishedYear { get; set; }

    public virtual Publisher? Publisher { get; set; }

}



