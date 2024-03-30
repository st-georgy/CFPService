using CFPService.Domain.Entities;

namespace CFPService.Domain.Abstractions.Repositories
{
    public interface IDraftsRepository
    {
        Task<Draft> GetAsync(Guid authorId);
        Task<Draft> RemoveAsync(Guid authorId);
    }
}
