using System.ComponentModel.DataAnnotations;

namespace LibraryManagementMVC.DTOs;

public class PublisherCreateDto
{

    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
}
