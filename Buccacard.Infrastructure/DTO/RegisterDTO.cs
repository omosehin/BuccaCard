﻿using System.ComponentModel.DataAnnotations;

namespace Buccacard.Infrastructure.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

#nullable disable
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
#nullable restore
        public string? Password { get; set; }

        public UserRole Role { get; set; } = UserRole.User;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
    }

    public class ComfirmDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = String.Empty;
    }
}
