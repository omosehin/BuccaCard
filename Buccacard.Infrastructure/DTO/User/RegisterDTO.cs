using System.ComponentModel.DataAnnotations;

namespace Buccacard.Infrastructure.DTO.User
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
    }

    public class ComfirmDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
