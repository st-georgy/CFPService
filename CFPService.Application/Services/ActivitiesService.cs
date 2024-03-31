using CFPService.Domain.Abstractions.Services;
using CFPService.Domain.Entities.Enums;

namespace CFPService.Application.Services
{
    public class ActivitiesService : IActivitiesService
    {
        public IEnumerable<Activity> GetActivities()
            => Enum.GetValues(typeof(Activity)).Cast<Activity>();
    }
}
