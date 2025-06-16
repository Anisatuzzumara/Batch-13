using System.ComponentModel.DataAnnotations;

namespace AutoMapperBookStoreMVC.DTO
{
    public class AuthorDto
    {
        public int AuthorID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }

        public List<BookDto> Books { get; set; } = new List<BookDto>();

    }

}
