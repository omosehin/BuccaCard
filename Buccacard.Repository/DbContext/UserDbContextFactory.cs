using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Buccacard.Repository.DbContext
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder();
            var connectionString = "Data Source = VGG-LT-538; Initial Catalog = BuccacardDb; Integrated Security = True; MultiSubnetFailover=False";
            builder.UseSqlServer(connectionString);

            return new UserDbContext(builder.Options);
        }
    }
}
