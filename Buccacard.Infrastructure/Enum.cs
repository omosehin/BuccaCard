using HermesApp.Infrastructure.Dictionary;

namespace Buccacard.Infrastructure
{
    public enum UserRole
    {
        [EnumDisplayName(DisplayName = "Super Admin")]
        SuperAdmin = 1,
        [EnumDisplayName(DisplayName = "Admin")]
        Admin,
        [EnumDisplayName(DisplayName = "User")]
        User, 
    }
}
