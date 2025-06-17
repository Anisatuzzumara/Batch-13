using System.ComponentModel.DataAnnotations;
using LibraryManagementMVC.Models;

namespace LibraryManagementMVC.Models
{
    public class Publisher
    {
        [Required(ErrorMessage = "PubId cannot empty")]
        public int PubId { get; set; }

        [Required(ErrorMessage = "Name cannot empty")]
        [StringLength(150, ErrorMessage = "Name cannot more than 150 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(100, ErrorMessage = "Country cannot more than 100 characters")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City cannot more than 100 characters")]
        public string City  { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email cannot more than 100 characters")]
        public string Email { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }

}