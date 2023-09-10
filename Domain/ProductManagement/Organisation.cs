using Buccacard.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Buccacard.Domain.ProductManagement
{
    public class Organisation : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
        public List<Card> Cards { get; set; } = null;
        public OrganisationType  OrganisationType { get; set; }
#nullable disable
        public List<Address> Addresses  { get; set; }

        public int AdminAppUser { get; set; } 

    }
}
