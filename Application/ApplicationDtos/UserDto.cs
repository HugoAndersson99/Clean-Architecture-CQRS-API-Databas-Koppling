using System.ComponentModel.DataAnnotations;

namespace Application.ApplicationDtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "UserName is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "UserName must be between 2 and 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Password must be at least 3 characters long.")]
        public string Password { get; set; }
    }
}
