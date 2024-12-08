using System.ComponentModel.DataAnnotations;

namespace Application.ApplicationDtos
{
    public class AuthorDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}
