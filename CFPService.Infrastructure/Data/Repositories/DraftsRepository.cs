using CFPService.Domain.Abstractions.Data;
using CFPService.Domain.Abstractions.Repositories;
using CFPService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CFPService.Infrastructure.Data.Repositories
{
    public class DraftsRepository(IServiceDbContext context) : IDraftsRepository
    {
        private readonly IServiceDbContext _context = context;

        public async Task<Draft?> GetAsync(Guid applicationId)
            => await _context.Drafts.AsNoTracking().FirstOrDefaultAsync(d => d.ApplicationId == applicationId);

        public async Task<Draft?> GetByAuthorIdAsync(Guid authorId)
            => await _context.Drafts.AsNoTracking().FirstOrDefaultAsync(d => d.AuthorId == authorId);

        public async Task RemoveAsync(Guid authorId)
            => await _context.Drafts.Where(d => d.AuthorId == authorId).ExecuteDeleteAsync();
    }
}
