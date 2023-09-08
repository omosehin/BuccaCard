using Microsoft.AspNetCore.Identity;

namespace Buccacard.Domain
{
    public class AppUser : IdentityUser
    {
        #nullable disable
        public string FirstName { get; set; }

        #nullable restore
        public string? LastName { get; set; } = string.Empty;

    }
}
