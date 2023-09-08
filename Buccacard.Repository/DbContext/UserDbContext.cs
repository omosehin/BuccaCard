using Buccacard.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Buccacard.Repository.DbContext
{
    public class UserDbContext : IdentityDbContext<AppUser> 
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {
        }
        public UserDbContext()
        {
            DbContextOptions<UserDbContext> options = new DbContextOptions<UserDbContext>();
        }
        public DbSet<AppUser> AppUsers { get; set; } 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
