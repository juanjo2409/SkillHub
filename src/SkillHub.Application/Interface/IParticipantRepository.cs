using SkillHub.Domain.Entities;

namespace SkillHub.Application.Interface;

public interface IParticipantRepository
{
    Task<Participant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Participant?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default);
    Task AddAsync(Participant participant, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
