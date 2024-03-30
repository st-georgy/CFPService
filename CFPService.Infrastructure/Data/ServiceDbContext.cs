using CFPService.Domain.Abstractions.Data;
using CFPService.Domain.Entities;
using CFPService.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CFPService.Infrastructure.Data
{
    public class ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : DbContext(options), IServiceDbContext
    {
        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Draft> Drafts { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
            modelBuilder.ApplyConfiguration(new DraftConfiguration());
        }
    }
}
