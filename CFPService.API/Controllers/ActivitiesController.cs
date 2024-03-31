using System.ComponentModel;
using CFPService.API.Contracts.Responses;
using CFPService.Domain.Abstractions.Services;
using CFPService.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ActivitiesController(IActivitiesService activitiesService) : Controller
    {
        private readonly IActivitiesService _activitiesService = activitiesService;

        [HttpGet]
        public ActionResult<IEnumerable<ActivityResponse>> GetActivities()
        {
            var activities = _activitiesService.GetActivities();
            var response = activities.Select(activity => new ActivityResponse(activity.ToString(), GetDescription(activity)));

            return Ok(response);
        }

        private static string GetDescription(Activity activity)
        {
            var field = typeof(Activity).GetField(activity.ToString());
            if (field is null)
                return "";

            var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? "";
        }
    }
}
