using System.Text.Json.Serialization;

namespace Buccacard.Infrastructure.DTO.Product
{
    public class CreateCard
    {
#nullable disable
        public string Name { get; set; }
        public string AppUserId { get; set; }
        public CardType CardType { get; set; }
#nullable restore
        public int? OrganisationId { get; set; } //individual cardtype won't be categorise as organisation
        public double CreditLimit { get; set; }

        [JsonIgnore]
        public Guid SerialNumber { get; set; } = Guid.NewGuid();

    }
}
