using CFPService.Domain.Entities.Enums;

namespace CFPService.Domain.Abstractions.Services
{
    public interface IActivitiesService
    {
        Task<IEnumerable<Activity>> GetActivities();
    }
}
