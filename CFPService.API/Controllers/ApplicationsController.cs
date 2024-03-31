using CFPService.API.Contracts.Requests;
using CFPService.API.Contracts.Responses;
using CFPService.Domain.Abstractions.Services;
using CFPService.Domain.Entities;
using CFPService.Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.API.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ApplicationsController(IApplicationsService applicationsService) : Controller
    {
        private readonly IApplicationsService _applicationsService = applicationsService;

        [HttpPost]
        public async Task<ActionResult<ApplicationResponse>> CreateApplication(CreateApplicationRequest request)
        {
            try
            {
                if ((string.IsNullOrWhiteSpace(request.Description)
                    && string.IsNullOrWhiteSpace(request.Name)
                    && string.IsNullOrWhiteSpace(request.Outline)
                    && string.IsNullOrWhiteSpace(request.Activity))
                    || request.Author == Guid.Empty)
                    return BadRequest("Author Id and at least one field must be specified.");

                Activity? activity = null;
                if (request.Activity is not null)
                    activity = (Activity)Enum.Parse(typeof(Activity), request.Activity);

                var application = await _applicationsService.CreateApplicationAsync(request.Author,
                    activity, DateTime.UtcNow, request.Name, request.Description, request.Outline)!;

                return Ok(new ApplicationResponse(application.Id,
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

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ApplicationResponse>> UpdateApplication(UpdateApplicationRequest request, Guid id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Description)
                        && string.IsNullOrWhiteSpace(request.Name)
                        && string.IsNullOrWhiteSpace(request.Outline)
                        && string.IsNullOrWhiteSpace(request.Activity))
                    return BadRequest("At least one field must be specified.");

                Activity? activity = null;
                if (request.Activity is not null)
                    activity = (Activity)Enum.Parse(typeof(Activity), request.Activity);

                var application = await _applicationsService.UpdateApplicationAsync(id,
                    activity,
                    request.Name,
                    request.Description,
                    request.Outline);

                return Ok(new ApplicationResponse(application!.Id,
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

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteApplication(Guid id)
        {
            try
            {
                await _applicationsService.DeleteApplicationAsync(id);
                return Ok();
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

        [HttpPost("{id:guid}/submit")]
        public async Task<ActionResult> SubmitApplication(Guid id)
        {
            try
            {
                await _applicationsService.SubmitApplicationAsync(id);
                return Ok();
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationResponse>>> GetApplicationsByDate(DateTime? submittedAfter, DateTime? unsubmittedOlder)
        {
            try
            {
                if (submittedAfter is not null && unsubmittedOlder is not null)
                    return BadRequest("Impossible to get submitted and unsubmitted applications simultaneously");

                IEnumerable<Domain.Entities.Application>? applications = null;

                if (submittedAfter is not null)
                    applications = await _applicationsService.GetSubmittedApplicationsAfterDateAsync(submittedAfter.Value.ToUniversalTime());
                else if (unsubmittedOlder is not null)
                    applications = await _applicationsService.GetUnsubmittedApplicationsAfterDateAsync(unsubmittedOlder.Value.ToUniversalTime());

                if (applications is null || !applications.Any())
                    return Ok();

                var response = applications.Select(application => new ApplicationResponse(application.Id,
                    application.AuthorId,
                    application.Activity.ToString() ?? "",
                    application.Name ?? "",
                    application.Description ?? "",
                    application.Outline ?? ""));
                    
                return Ok(response);
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

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ApplicationResponse>> GetApplicationById(Guid id)
        {
            try
            {
                var application = await _applicationsService.GetApplicationAsync(id);

                if (application is null)
                    return NotFound($"Application with id {id} was not found");

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
