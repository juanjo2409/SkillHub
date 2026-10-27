namespace SkillHub.Application.DTOs.Workshops;

public record WorkshopResponseDto(
    Guid Id,
    string Code,
    string Title,
    string Description,
    string InstructorName,
    string InstructorEmail,
    string InstructorSpecialty,
    DateTime StartDate,
    DateTime EndDate,
    int MaxCapacity,
    int ActiveEnrollmentsCount,
    int AvailableSpots);
