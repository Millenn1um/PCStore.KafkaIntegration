using Microsoft.EntityFrameworkCore;
using PCStore.Models;
namespace PCStore.Data


{
    public class PcShopContext : DbContext
    {
        public PcShopContext(DbContextOptions<PcShopContext> options) : base(options) { }

        public DbSet<CPU> CPUs { get; set; }
        public DbSet<GPU> GPUs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CPU>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<GPU>()
                .Property(g => g.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
