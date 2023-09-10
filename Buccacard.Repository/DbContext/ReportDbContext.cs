using Buccacard.Domain.ProductManagement;
using Buccacard.Domain.ReportManagement;
using Microsoft.EntityFrameworkCore;

#nullable disable
public class ReportDbContext : DbContext
{
    public ReportDbContext(DbContextOptions options) : base(options)
    {
    }
    public ReportDbContext()
    {
        DbContextOptions<ReportDbContext> options = new DbContextOptions<ReportDbContext>();
    }

    public DbSet<ReportTracker> ReportTrackers { get; set; }

}

