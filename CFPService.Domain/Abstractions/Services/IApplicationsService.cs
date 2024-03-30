using CFPService.Domain.Entities;
using CFPService.Domain.Entities.Enums;

namespace CFPService.Domain.Abstractions.Services
{
    public interface IApplicationsService
    {
        Task<Application> GetApplicationAsync(Guid id);
        Task<Guid> CreateApplicationAsync(Guid authorId, Activity? activity, DateTime createdDate, DateTime? submittedDate, string? title, string? description, string? outline);
        Task<Application> UpdateApplicationAsync(Guid applicationId, Activity activity, string? name, string? description, string? outline);
        Task DeleteApplicationAsync(Guid id);
        Task SubmitApplicationAsync(Guid id);
        Task<IEnumerable<Application>> GetSubmittedApplicationsAfterAsync(DateTime submittedAfter);
        Task<IEnumerable<Application>> GetUnsubmittedApplicationsOlderThanAsync(DateTime unsubmittedOlderThan);
        Task<Application> GetCurrentUnsubmittedApplicationAsync(Guid userId);
    }
}
