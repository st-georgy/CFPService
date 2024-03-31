namespace CFPService.API.Contracts.Requests
{
    public record UpdateApplicationRequest(string? Activity,
        string? Name,
        string? Description,
        string? Outline);
}
