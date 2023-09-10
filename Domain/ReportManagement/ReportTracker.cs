using System.ComponentModel.DataAnnotations;

namespace Buccacard.Domain.ReportManagement
{
    public class ReportTracker : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastRun { get; set; } = default;
    }
}
