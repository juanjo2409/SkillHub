using Microsoft.EntityFrameworkCore;
using SkillHub.Application.Interface;
using SkillHub.Domain.Entities;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Infrastructure.Persistence.Repositories;

public class WorkshopRepository : IWorkshopRepository
{
    private readonly ApplicationDbContext _context;

    public WorkshopRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Workshop?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Workshops
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    public async Task<Workshop?> GetByCodeWithDetailsAsync(string code, CancellationToken cancellationToken = default)
    {
        var workshopCode = new WorkshopCode(code);
        return await _context.Workshops
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .FirstOrDefaultAsync(w => w.Code == workshopCode, cancellationToken);
    }

    public async Task<IReadOnlyList<Workshop>> GetUpcomingCatalogAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.Workshops
            .AsNoTracking()
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .Where(w => w.Schedule.StartDate > now)
            .OrderBy(w => w.Schedule.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Workshop>> GetTopByActiveEnrollmentsAsync(CancellationToken cancellationToken = default) =>
        await _context.Workshops
            .AsNoTracking()
            .Include(w => w.Instructor)
            .Include(w => w.Enrollments)
            .ToListAsync(cancellationToken);

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var workshopCode = new WorkshopCode(code);
        return await _context.Workshops.AnyAsync(w => w.Code == workshopCode, cancellationToken);
    }

    public async Task AddAsync(Workshop workshop, CancellationToken cancellationToken = default) =>
        await _context.Workshops.AddAsync(workshop, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
