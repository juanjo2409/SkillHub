using SkillHub.Domain.Entities;

namespace SkillHub.Application.Interface;

public interface IEnrollmentRepository
{
    Task<Enrollment?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Enrollment?> GetByParticipantAndWorkshopAsync(Guid participantId, Guid workshopId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Enrollment>> GetByParticipantIdAsync(Guid participantId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Enrollment>> GetActiveByParticipantIdAsync(Guid participantId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Enrollment>> GetByWorkshopIdAsync(Guid workshopId, CancellationToken cancellationToken = default);
    Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
