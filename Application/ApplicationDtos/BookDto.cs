using System.ComponentModel.DataAnnotations;

namespace Application.ApplicationDtos
{
    public class BookDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 200 characters.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        public AuthorDto? Author { get; set; }
    }
}
