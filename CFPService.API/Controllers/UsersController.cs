using CFPService.API.Contracts.Responses;
using CFPService.Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsersController(IApplicationsService applicationsService) : Controller
    {
        private readonly IApplicationsService _applicationsService = applicationsService;

        [HttpGet("{id:guid}/currentapplication")]
        public async Task<ActionResult<ApplicationResponse>> GetCurrentApplication(Guid id)
        {
            try
            {
                var application = await _applicationsService.GetCurrentUnsubmittedApplicationAsync(id);

                if (application is null)
                    return Ok();

                return Ok(new ApplicationResponse(
                    application.Id,
                    application.AuthorId,
                    application.Activity.ToString() ?? "",
                    application.Name ?? "",
                    application.Description ?? "",
                    application.Outline ?? ""));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Invalid operation: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                return BadRequest($"Invalid argument(s): {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
