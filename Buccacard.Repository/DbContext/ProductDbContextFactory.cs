using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Buccacard.Repository.DbContext
{
    public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args) 
        {
            var builder = new DbContextOptionsBuilder();
            var connectionString = "Data Source = VGG-LT-538; Initial Catalog = BuccacardProductDb; Integrated Security = True; MultiSubnetFailover=False";
            builder.UseSqlServer(connectionString);

            return new ProductDbContext(builder.Options);
        }
    }
}
