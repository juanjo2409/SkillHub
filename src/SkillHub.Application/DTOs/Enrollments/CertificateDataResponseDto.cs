namespace SkillHub.Application.DTOs.Enrollments;

public record CertificateDataResponseDto(
    string OrganizationName,
    string DocumentTitle,
    string ParticipantName,
    string ParticipantEmail,
    string WorkshopCode,
    string WorkshopTitle,
    string InstructorName,
    DateTime StartDate,
    DateTime EndDate,
    DateTime EnrollmentDate,
    DateTime GeneratedAt);
