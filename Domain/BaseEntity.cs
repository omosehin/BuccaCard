namespace Buccacard.Domain
{
    public abstract class BaseEntity
    {
        public bool IsActive { get; set; } = false;
        public DateTime Created { get; set; } 
        public DateTime Modified { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
    }
}
