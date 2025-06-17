using System.ComponentModel.DataAnnotations;

namespace LibraryManagementMVC.DTOs;

public class PublisherCreateUpdateDto
{
    public int PubId { get; set; }

    [Required(ErrorMessage = "Name cannot be empty")]
    [StringLength(150, ErrorMessage = "Name cannot be more than 150 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required")]
    [StringLength(100, ErrorMessage = "Country cannot be more than 100 characters")]
    public string? Country { get; set; }
}
