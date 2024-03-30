using CFPService.Domain.Abstractions.Data;
using CFPService.Domain.Abstractions.Repositories;
using CFPService.Domain.Entities;
using CFPService.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace CFPService.Infrastructure.Data.Repositories
{
    public class ApplicationsRepository(IServiceDbContext context) : IApplicationsRepository
    {
        private readonly IServiceDbContext _context = context;

        public async Task<Guid> CreateAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();

            return application.Id;
        }

        public async Task<Guid> DeleteAsync(Guid id)
        {
            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id)
                    ?? throw new ArgumentException("Application with stated Id doesn't exist.", nameof(id));

            await _context.Applications
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync();

            return id;
        }

        public async Task<Application?> GetAsync(Guid id)
        {
            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            return application;
        }

        public async Task<IEnumerable<Application>> GetSubmittedAfterDateAsync(DateTime submittedAfter)
        {
            var applications = await _context.Applications
                .AsNoTracking()
                .Where(a => a.SubmittedDate > submittedAfter)
                .ToListAsync();

            return applications;
        }

        public async Task<IEnumerable<Application>> GetUnsubmittedAfterDateAsync(DateTime unsubmittedOlderThan)
        {
            var applications = await _context.Applications
                .AsNoTracking()
                .Where(a => a.CreatedDate > unsubmittedOlderThan)
                .ToListAsync();

            return applications;
        }

        public async Task<Application?> UpdateAsync(Guid id, Guid? authorId, Activity? activity, string? title, string? description, string? outline)
        {
            var application = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (authorId is null && activity is null && title is null && description is null && outline is null)
                return application;

            await _context.Applications
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(a => a.AuthorId, a => authorId ?? a.AuthorId)
                    .SetProperty(a => a.Activity, a => activity ?? a.Activity)
                    .SetProperty(a => a.Title, a => title ?? a.Title)
                    .SetProperty(a => a.Description, a => description ?? a.Description)
                    .SetProperty(a => a.Outline, a => outline ?? a.Outline));

            return application;
        }

        public async Task<Guid> SubmitAsync(Guid id)
        {
            await _context.Applications
                .Where(a => a.Id == id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(a => a.SubmittedDate, a => DateTime.UtcNow));

            return id;
        }
    }
}
