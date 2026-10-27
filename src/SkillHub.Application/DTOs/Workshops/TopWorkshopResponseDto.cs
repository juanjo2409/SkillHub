namespace SkillHub.Application.DTOs.Workshops;

public record TopWorkshopResponseDto(
    Guid Id,
    string Code,
    string Title,
    string InstructorName,
    DateTime StartDate,
    DateTime EndDate,
    int MaxCapacity,
    int ActiveEnrollmentsCount,
    int AvailableSpots);
