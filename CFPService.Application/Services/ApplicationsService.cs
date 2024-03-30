using CFPService.Domain.Abstractions.Repositories;
using CFPService.Domain.Abstractions.Services;
using CFPService.Domain.Entities.Enums;

namespace CFPService.Application.Services
{
    public class ApplicationsService(IApplicationsRepository applicationsRepository, IDraftsRepository draftsRepository) : IApplicationsService
    {
        private readonly IApplicationsRepository _applicationsRepository = applicationsRepository;
        private readonly IDraftsRepository _draftsRepository = draftsRepository;

        public async Task<Domain.Entities.Application> CreateApplicationAsync(Guid authorId, Activity? activity, DateTime createdDate, string? title, string? description, string? outline)
        {
            var guid = Guid.NewGuid();
            var application = Domain.Entities.Application.Create(guid, authorId, activity, createdDate, null, title, description, outline);

            await _applicationsRepository.CreateAsync(application);

            return application;
        }

        public async Task<Guid> DeleteApplicationAsync(Guid id)
        {
            _ = await _applicationsRepository.GetAsync(id)
                ?? throw new ArgumentException("Application with stated id was not found");

            var draft = await _draftsRepository.GetAsync(id);
            
            if (draft is not null)
                throw new InvalidOperationException("Submitted application can not be deleted.");

            return await _applicationsRepository.DeleteAsync(id);
        }

        public async Task<Domain.Entities.Application?> GetApplicationAsync(Guid id)
            => await _applicationsRepository.GetAsync(id);

        public async Task<Domain.Entities.Application?> GetCurrentUnsubmittedApplicationAsync(Guid userId)
        {
            var draft = await _draftsRepository.GetByAuthorIdAsync(userId);

            if (draft is null)
                return null;

            var application = await _applicationsRepository.GetAsync(draft.ApplicationId);

            return application;
        }

        public async Task<IEnumerable<Domain.Entities.Application>> GetSubmittedApplicationsAfterDateAsync(DateTime submittedAfter)
            => await _applicationsRepository.GetSubmittedAfterDateAsync(submittedAfter);

        public async Task<IEnumerable<Domain.Entities.Application>> GetUnsubmittedApplicationsAfterDateAsync(DateTime unsubmittedOlderThan)
            => await _applicationsRepository.GetUnsubmittedAfterDateAsync(unsubmittedOlderThan);

        public async Task SubmitApplicationAsync(Guid id)
        {
            var application = await _applicationsRepository.GetAsync(id);

            if (application is null)
                return;

            var draft = await _draftsRepository.GetAsync(id);

            if (draft is null)
                return;

            await _applicationsRepository.SubmitAsync(id);
            await _draftsRepository.RemoveAsync(id);
        }

        public async Task<Domain.Entities.Application?> UpdateApplicationAsync(Guid applicationId, Activity activity, string? name, string? description, string? outline)
            => await _applicationsRepository.UpdateAsync(applicationId, activity, name, description, outline);  
    }
}
