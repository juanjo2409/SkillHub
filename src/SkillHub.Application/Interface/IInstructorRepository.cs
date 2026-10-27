using SkillHub.Domain.Entities;

namespace SkillHub.Application.Interface;

public interface IInstructorRepository
{
    Task<Instructor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Instructor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Instructor?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default);
    Task AddAsync(Instructor instructor, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
