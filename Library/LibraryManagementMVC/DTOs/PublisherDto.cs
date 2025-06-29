using System.ComponentModel.DataAnnotations;
using LibraryManagementMVC.DTOs;
namespace LibraryManagementMVC.DTOs;

public class PublisherDto
{
    public int PubId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public int BooksCount { get; set; }
    

}

    