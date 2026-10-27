using System.ComponentModel.DataAnnotations;
using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;

namespace SkillHub.Web.Models;

public class UserProfileViewModel
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [StringLength(120, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 120 caracteres.")]
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Specialty { get; set; }

    public string? CurrentPassword { get; set; }

    [MinLength(6, ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres.")]
    public string? NewPassword { get; set; }

    public bool IsEmployee => Role == "Employee";

    public IReadOnlyList<EnrollmentResponseDto> MyEnrollments { get; init; } = Array.Empty<EnrollmentResponseDto>();
    public IReadOnlyList<WorkshopResponseDto> MyInstructorWorkshops { get; init; } = Array.Empty<WorkshopResponseDto>();
    public Dictionary<Guid, IReadOnlyList<EnrollmentResponseDto>> EnrollmentsByWorkshop { get; init; } = new();
    public IReadOnlyList<WorkshopResponseDto> AllWorkshops { get; init; } = Array.Empty<WorkshopResponseDto>();
    public IReadOnlyList<TopWorkshopResponseDto> TopWorkshops { get; init; } = Array.Empty<TopWorkshopResponseDto>();
}
