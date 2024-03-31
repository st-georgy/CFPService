using CFPService.Domain.Entities;

namespace CFPService.Domain.Abstractions.Repositories
{
    public interface IDraftsRepository
    {
        Task CreateAsync(Draft draft);
        Task<Draft?> GetAsync(Guid applicationId);
        Task<Draft?> GetByAuthorIdAsync(Guid authorId);
        Task<IEnumerable<Draft>> GetAllAsync();
        Task RemoveAsync(Guid applicationId);
        Task RemoveByAuthorAsync(Guid authorId);
    }
}
