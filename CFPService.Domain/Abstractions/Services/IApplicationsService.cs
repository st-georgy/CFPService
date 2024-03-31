using CFPService.Domain.Entities;
using CFPService.Domain.Entities.Enums;

namespace CFPService.Domain.Abstractions.Services
{
    public interface IApplicationsService
    {
        Task<Application> CreateApplicationAsync(Guid authorId, Activity? activity, DateTime createdDate, string? name, string? description, string? outline);
        Task<Application?> UpdateApplicationAsync(Guid applicationId, Activity? activity, string? name, string? description, string? outline);
        Task<Guid> DeleteApplicationAsync(Guid id);
        Task SubmitApplicationAsync(Guid id);
        Task<IEnumerable<Application>> GetSubmittedApplicationsAfterDateAsync(DateTime submittedAfter);
        Task<IEnumerable<Application>> GetUnsubmittedApplicationsAfterDateAsync(DateTime unsubmittedOlderThan);
        Task<Application?> GetCurrentUnsubmittedApplicationAsync(Guid userId);
        Task<Application?> GetApplicationAsync(Guid id);
    }
}
