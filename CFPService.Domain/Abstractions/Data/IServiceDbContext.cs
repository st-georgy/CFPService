using CFPService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CFPService.Domain.Abstractions.Data
{
    public interface IServiceDbContext
    {
        DbSet<Application> Applications { get; set; }
        DbSet<Draft> Drafts { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
