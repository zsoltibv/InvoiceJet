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
            modelBuilder.Entity<User>()
                .HasOne(u => u.ActiveUserFirm)
                .WithMany() // No inverse navigation property defined in UserFirm
                .HasForeignKey(u => u.ActiveUserFirmId)
                .IsRequired(false); // Assuming it can be null
        }

        public DbSet<User> User { get; set; }
        public DbSet<Firm> Firm { get; set; }
        public DbSet<BankAccount> BankAccount { get; set; }
        public DbSet<UserFirm> UserFirm { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
