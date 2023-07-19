using AmlApiMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace AmlApiMvc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AmlRequest> AmlRequests { get; set; }
        public DbSet<AmlRecheckRequest> AmlRecheckRequests { get; set; }
        public DbSet<AmlResponse> AmlResponses { get; set; }
        public DbSet<WalletAddress> WalletAddresses { get; set; }
        public DbSet<NetworkType> NetworkTypes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AmlRequest>().ToTable("AmlRequests");
            modelBuilder.Entity<AmlRecheckRequest>().ToTable("AmlReCheckRequests");
            modelBuilder.Entity<AmlResponse>().ToTable("AmlResponses");
            modelBuilder.Entity<WalletAddress>().ToTable("WalletAddresses");
            modelBuilder.Entity<NetworkType>().ToTable("NetworkTypes");
        }
    }
}
