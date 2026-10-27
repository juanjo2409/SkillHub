using SkillHub.Application.DTOs.Enrollments;

namespace SkillHub.Application.Interface;

public interface IEnrollmentService
{
    Task<EnrollmentResponseDto> EnrollParticipantAsync(string identityUserId, CreateEnrollmentRequestDto request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EnrollmentResponseDto>> GetMyEnrollmentsAsync(string identityUserId, CancellationToken cancellationToken = default);
    Task<EnrollmentResponseDto> CancelEnrollmentAsync(Guid enrollmentId, string currentUserId, bool isEmployee, CancellationToken cancellationToken = default);
    Task<ImportEnrollmentResponseDto> ImportFromExcelAsync(Stream excelStream, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateCertificatePdfAsync(Guid enrollmentId, string currentUserId, bool isEmployee, CancellationToken cancellationToken = default);
}
