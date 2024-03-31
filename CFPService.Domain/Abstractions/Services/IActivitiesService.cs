using CFPService.Domain.Entities.Enums;

namespace CFPService.Domain.Abstractions.Services
{
    public interface IActivitiesService
    {
        IEnumerable<Activity> GetActivities();
    }
}
