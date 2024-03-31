using System.ComponentModel.DataAnnotations;
using CFPService.Domain.Entities.Enums;

namespace CFPService.API.Contracts.Requests
{
    public record CreateApplicationRequest(
        [Required] Guid Author,
        string? Activity,
        string? Name,
        string? Description,
        string? Outline);
}
