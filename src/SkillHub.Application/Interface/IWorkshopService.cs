using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;

namespace SkillHub.Application.Interface;

public interface IWorkshopService
{
    Task<IReadOnlyList<WorkshopResponseDto>> GetUpcomingCatalogAsync(CancellationToken cancellationToken = default);
    Task<WorkshopResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<WorkshopResponseDto> CreateAsync(CreateWorkshopRequestDto request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EnrollmentResponseDto>> GetWorkshopEnrollmentsAsync(Guid workshopId, CancellationToken cancellationToken = default);
    Task<(byte[] FileBytes, string FileName)> ExportWorkshopEnrollmentsExcelAsync(Guid workshopId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TopWorkshopResponseDto>> GetTopWorkshopsAsync(CancellationToken cancellationToken = default);
}
