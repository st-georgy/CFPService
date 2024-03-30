using CFPService.Domain.Entities;

namespace CFPService.Domain.Abstractions.Repositories
{
    public interface IDraftsRepository
    {
        Task<Draft?> GetAsync(Guid applicationId);
        Task<Draft?> GetByAuthorIdAsync(Guid authorId);
        Task RemoveAsync(Guid authorId);
    }
}
