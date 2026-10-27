using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;

namespace SkillHub.Web.Models;

public class UserProfileViewModel
{
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public bool IsEmployee => Role == "Employee";

    public IReadOnlyList<EnrollmentResponseDto> MyEnrollments { get; init; } = Array.Empty<EnrollmentResponseDto>();
    public IReadOnlyList<WorkshopResponseDto> MyInstructorWorkshops { get; init; } = Array.Empty<WorkshopResponseDto>();
    public Dictionary<Guid, IReadOnlyList<EnrollmentResponseDto>> EnrollmentsByWorkshop { get; init; } = new();
    public IReadOnlyList<WorkshopResponseDto> AllWorkshops { get; init; } = Array.Empty<WorkshopResponseDto>();
    public IReadOnlyList<TopWorkshopResponseDto> TopWorkshops { get; init; } = Array.Empty<TopWorkshopResponseDto>();
}
