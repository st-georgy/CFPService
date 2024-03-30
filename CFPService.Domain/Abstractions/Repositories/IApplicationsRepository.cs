using CFPService.Domain.Entities;

namespace CFPService.Domain.Abstractions.Repositories
{
    public interface IApplicationsRepository
    {
        Task<Application> GetAsync(Guid id);
        Task<Guid> CreateAsync(Application application);
        Task<Application> UpdateAsync(Application application);
        Task DeleteAsync(Guid id);
        Task SubmitAsync(Guid id);
        Task<IEnumerable<Application>> GetSubmittedAfterAsync(DateTime submittedAfter);
        Task<IEnumerable<Application>> GetUnsubmittedOlderThanAsync(DateTime unsubmittedOlderThan);
        Task<Application> GetCurrentUnsubmittedAsync(Guid authorId);
    }
}
