using Buccacard.Domain.ProductManagement;
using Microsoft.EntityFrameworkCore;

#nullable disable
public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions options) : base(options)
    {
    }
    public ProductDbContext()
    {
        DbContextOptions<ProductDbContext> options = new DbContextOptions<ProductDbContext>();
    }

    public DbSet<Card> Cards { get; set; }
    public DbSet<Organisation>  Organisations { get; set; }

    public DbSet<Address>  Addresses { get; set; }
}

