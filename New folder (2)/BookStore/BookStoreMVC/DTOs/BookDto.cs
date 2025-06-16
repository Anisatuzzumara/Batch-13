using System.ComponentModel.DataAnnotations;

namespace AutoMapperBookStoreMVC.DTO
{
    public class BookDto
    {
        public int BookID { get; set; }
        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string AuthorName { get; set; } = string.Empty;



    }
}