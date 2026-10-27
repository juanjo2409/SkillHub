namespace SkillHub.Application.DTOs.Enrollments;

public record EnrollmentResponseDto(
    Guid Id,
    Guid WorkshopId,
    string WorkshopCode,
    string WorkshopTitle,
    string InstructorName,
    DateTime StartDate,
    DateTime EndDate,
    Guid ParticipantId,
    string ParticipantName,
    string ParticipantEmail,
    DateTime EnrollmentDate,
    string Status,
    DateTime? CancelledAt,
    string? CancelledByUserId);
