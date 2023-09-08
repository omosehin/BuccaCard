using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Buccacard.Infrastructure.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [JsonPropertyName("email")]
        public string Email { get; set; } 

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(99, ErrorMessage = "Password should not be more than 99 characters")]
        [JsonPropertyName("password")]
        public string PassWord { get; set; }
    }

    public class Token
    {
        public string Jwt { get; set; }
        public string RefreshToken { get; set; }
    }
}
