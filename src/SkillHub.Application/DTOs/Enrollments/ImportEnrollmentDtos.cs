namespace SkillHub.Application.DTOs.Enrollments;

public record ImportEnrollmentRowRequestDto(
    int RowNumber,
    string ParticipantEmail,
    string WorkshopCode);

public record RejectedEnrollmentRowResponseDto(
    int RowNumber,
    string ParticipantEmail,
    string WorkshopCode,
    string Reason);

public record ImportEnrollmentResponseDto(
    int TotalEnrolled,
    IReadOnlyList<RejectedEnrollmentRowResponseDto> RejectedRows);
