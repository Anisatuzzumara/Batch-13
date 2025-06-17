using System.ComponentModel.DataAnnotations;
namespace LibraryManagementMVC.DTOs;

public class PublisherDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }

    [Display(Name = "Jumlah Buku")]

    public int BooksCount { get; set; }

}

    