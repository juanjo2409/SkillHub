using SkillHub.Application.DTOs.Workshops;

namespace SkillHub.Web.Models;

public class WorkshopCatalogViewModel
{
    public IReadOnlyList<WorkshopResponseDto> Workshops { get; init; } = Array.Empty<WorkshopResponseDto>();
    public HashSet<Guid> ActiveEnrolledWorkshopIds { get; init; } = new();
}
