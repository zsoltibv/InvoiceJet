using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Config
{
    public class FacturilaDbContext : DbContext
    {
        public FacturilaDbContext(DbContextOptions<FacturilaDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Firm> Firm { get; set; }
    }
}
