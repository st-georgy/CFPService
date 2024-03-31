using CFPService.Domain.Abstractions.Repositories;
using CFPService.Domain.Abstractions.Services;
using CFPService.Domain.Entities;
using CFPService.Domain.Entities.Enums;

namespace CFPService.Application.Services
{
    public class ApplicationsService(IApplicationsRepository applicationsRepository, IDraftsRepository draftsRepository) : IApplicationsService
    {
        private readonly IApplicationsRepository _applicationsRepository = applicationsRepository;
        private readonly IDraftsRepository _draftsRepository = draftsRepository;

        public async Task<Domain.Entities.Application> CreateApplicationAsync(Guid authorId, Activity? activity, DateTime createdDate, string? name, string? description, string? outline)
        {
            var check_draft = await _draftsRepository.GetByAuthorIdAsync(authorId);

            if (check_draft is not null)
                throw new InvalidOperationException("User can have only 1 draft.");

            var applicationId = Guid.NewGuid();
            var application = Domain.Entities.Application.Create(applicationId, authorId, activity, createdDate, null, name, description, outline);
            var draft = Draft.Create(applicationId, authorId);

            await _applicationsRepository.CreateAsync(application);
            await _draftsRepository.CreateAsync(draft);

            return application;
        }

        public async Task<Guid> DeleteApplicationAsync(Guid id)
        {
            _ = await _applicationsRepository.GetAsync(id)
                ?? throw new ArgumentException("Application with stated id was not found");

            _ = await _draftsRepository.GetAsync(id)
                ?? throw new InvalidOperationException("Submitted application can not be deleted.");

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
        { 
            var applications = await _applicationsRepository.GetUnsubmittedAfterDateAsync(unsubmittedOlderThan);
            var draftIds = (await _draftsRepository.GetAllAsync()).Select(d => d.ApplicationId);

            return applications.Where(a => draftIds.Contains(a.Id));
        }

        public async Task SubmitApplicationAsync(Guid id)
        {
            var application = await _applicationsRepository.GetAsync(id)
                ?? throw new ArgumentException("Application with stated id was not found");

            if (application.Id == Guid.Empty
                || application.AuthorId == Guid.Empty
                || string.IsNullOrWhiteSpace(application.Name)
                || string.IsNullOrWhiteSpace(application.Description)
                || string.IsNullOrWhiteSpace(application.Outline)
                || application.Activity is null
                || !Enum.IsDefined(typeof(Activity), application.Activity))
                throw new InvalidOperationException("All application field must be filled.");

            var draft = await _draftsRepository.GetAsync(id);

            if (draft is null)
                return;

            await _applicationsRepository.SubmitAsync(id);
            await _draftsRepository.RemoveAsync(id);
        }

        public async Task<Domain.Entities.Application?> UpdateApplicationAsync(Guid applicationId, Activity? activity, string? name, string? description, string? outline)
        {
            _ = await _applicationsRepository.GetAsync(applicationId)
                ?? throw new ArgumentException("Application with stated id was not found");

            _ = await _draftsRepository.GetAsync(applicationId)
                ?? throw new InvalidOperationException("Submitted application can not be edited."); ;

            return await _applicationsRepository.UpdateAsync(applicationId, activity, name, description, outline);
        }
    }
}
