using Microsoft.AspNetCore.Identity;

namespace Buccacard.Domain.UserManagement
{
    public class AppUser : IdentityUser
    {
#nullable disable
        public string FirstName { get; set; } = string.Empty;

#nullable restore
        public string? LastName { get; set; } = string.Empty;

    }

    public class ApplicationRole : IdentityRole
    {
    }

}
