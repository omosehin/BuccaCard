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

    public enum CardType
    {
        Cooporate = 1,
        Individual 
    } 

    public enum OrganisationType
    {
        Family = 1,
        Hospital,
        Church,
        Airlines,
        Govermentt, 
        Vendor //Shprite,Bukahut,Pricepally
    }
}
