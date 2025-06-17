using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementMVC.Dtos;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;

    [Display(Name = "Publish Year")]
    public int PublishedYear { get; set; }
    
    [Display(Name = "Penerbit")]
    public string PublisherName { get; set; } = string.Empty;
}
