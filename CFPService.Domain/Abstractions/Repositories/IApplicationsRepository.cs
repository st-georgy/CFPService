using CFPService.Domain.Entities;
using CFPService.Domain.Entities.Enums;

namespace CFPService.Domain.Abstractions.Repositories
{
    public interface IApplicationsRepository
    {
        Task<Guid> CreateAsync(Application application);
        Task<Guid> DeleteAsync(Guid id);
        Task<Application?> GetAsync(Guid id);
        Task<IEnumerable<Application>> GetSubmittedAfterDateAsync(DateTime submittedAfter);
        Task<IEnumerable<Application>> GetUnsubmittedAfterDateAsync(DateTime unsubmittedOlderThan);
        Task<Application?> UpdateAsync(Guid id, Activity? activity, string? name, string? description, string? outline);
        Task<Guid> SubmitAsync(Guid id);
    }
}
