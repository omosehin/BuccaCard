using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Buccacard.Repository.DbContext
{
    public class ReportDbContextFactory : IDesignTimeDbContextFactory<ReportDbContext>
    {
        public ReportDbContext CreateDbContext(string[] args) 
        {
            var builder = new DbContextOptionsBuilder();
            var connectionString = "Data Source = VGG-LT-538; Initial Catalog = BuccacardProductDb; Integrated Security = True; MultiSubnetFailover=False";
            builder.UseSqlServer(connectionString);

            return new ReportDbContext(builder.Options); 
        }
    }
}
