using System.ComponentModel.DataAnnotations;

namespace BookStoreMVC.DTO;

public class AuthorCreateDto
{

    [Required(ErrorMessage = "Author Name cannot empty")]
    [StringLength(50, ErrorMessage = "Name cannot more than 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Bio is required")]
    [StringLength(100, ErrorMessage = "Bio Author cannot more than 100 characters")]
    public string? Bio { get; set; }

}
