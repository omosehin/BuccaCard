using System.ComponentModel.DataAnnotations;

namespace Buccacard.Domain.ProductManagement
{
    public class Address : BaseEntity
    {
        [Key]
        public int MyProperty { get; set; }
        public int State { get; set; }
        #nullable disable
        public string LocalGovernment { get; set; }
        public string Street { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
