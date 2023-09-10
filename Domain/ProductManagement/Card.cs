using Buccacard.Domain.UserManagement;
using Buccacard.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Buccacard.Domain.ProductManagement
{
    [Index(nameof(AppUserId), IsUnique = true)]
    public class Card : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public CardType CardType { get; set; } = CardType.Individual;
        public Organisation Organisation { get; set; } = null; //individual cardtype won't be categorise as organisation
        public double CreditLimit { get; set; }
        public Guid SerialNumber { get; set; } = Guid.NewGuid();
    }
}
