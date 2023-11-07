using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Config
{
    public class FacturilaDbContext : DbContext
    {
        public FacturilaDbContext(DbContextOptions<FacturilaDbContext> options) : base(options)
        {
        }

        public DbSet<FirmData> FirmData { get; set; }
    }
}
