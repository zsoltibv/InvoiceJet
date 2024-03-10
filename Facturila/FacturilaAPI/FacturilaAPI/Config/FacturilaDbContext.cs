using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Config
{
    public class FacturilaDbContext : DbContext
    {
        public FacturilaDbContext(DbContextOptions<FacturilaDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserFirm>()
                .HasKey(uf => new { uf.UserId, uf.FirmId }); 
        }

        public DbSet<User> User { get; set; }
        public DbSet<Firm> Firm { get; set; }
        public DbSet<UserFirm> UserFirm { get; set; }
    }
}
