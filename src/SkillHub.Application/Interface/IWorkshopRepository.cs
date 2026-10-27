using SkillHub.Domain.Entities;

namespace SkillHub.Application.Interface;

public interface IWorkshopRepository
{
    Task<Workshop?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Workshop?> GetByCodeWithDetailsAsync(string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Workshop>> GetUpcomingCatalogAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Workshop>> GetTopByActiveEnrollmentsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task AddAsync(Workshop workshop, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
