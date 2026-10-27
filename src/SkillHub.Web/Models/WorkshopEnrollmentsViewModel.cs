using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;

namespace SkillHub.Web.Models;

public class WorkshopEnrollmentsViewModel
{
    public required WorkshopResponseDto Workshop { get; init; }
    public IReadOnlyList<EnrollmentResponseDto> Enrollments { get; init; } = Array.Empty<EnrollmentResponseDto>();
}
