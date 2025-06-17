using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibraryManagementMVC.Dtos;

public class BookCreateUpdateDto
{
    public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;

        [Display(Name = "Tahun Terbit")]
        public int PublishedYear { get; set; }

        [Display(Name = "Penerbit")]
        public int PublisherId { get; set; }
        
        // Properti ini untuk menampung daftar pilihan penerbit di form dropdown.
        public IEnumerable<SelectListItem>? Publishers { get; set; }
}
