using Microsoft.EntityFrameworkCore;
using SkillHub.Application.Interface;
using SkillHub.Domain.Entities;
using SkillHub.Domain.Enums;

namespace SkillHub.Infrastructure.Persistence.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _context;

    public EnrollmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Enrollments
            .Include(e => e.Participant)
            .Include(e => e.Workshop)
                .ThenInclude(w => w.Instructor)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<Enrollment?> GetByParticipantAndWorkshopAsync(Guid participantId, Guid workshopId, CancellationToken cancellationToken = default) =>
        await _context.Enrollments
            .Include(e => e.Participant)
            .Include(e => e.Workshop)
                .ThenInclude(w => w.Instructor)
            .FirstOrDefaultAsync(e => e.ParticipantId == participantId && e.WorkshopId == workshopId, cancellationToken);

    public async Task<IReadOnlyList<Enrollment>> GetByParticipantIdAsync(Guid participantId, CancellationToken cancellationToken = default) =>
        await _context.Enrollments
            .AsNoTracking()
            .Include(e => e.Participant)
            .Include(e => e.Workshop)
                .ThenInclude(w => w.Instructor)
            .Where(e => e.ParticipantId == participantId)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Enrollment>> GetActiveByParticipantIdAsync(Guid participantId, CancellationToken cancellationToken = default) =>
        await _context.Enrollments
            .AsNoTracking()
            .Include(e => e.Workshop)
            .Where(e => e.ParticipantId == participantId && e.Status == EnrollmentStatus.Active)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Enrollment>> GetByWorkshopIdAsync(Guid workshopId, CancellationToken cancellationToken = default) =>
        await _context.Enrollments
            .AsNoTracking()
            .Include(e => e.Participant)
            .Include(e => e.Workshop)
                .ThenInclude(w => w.Instructor)
            .Where(e => e.WorkshopId == workshopId)
            .OrderByDescending(e => e.EnrollmentDate)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default) =>
        await _context.Enrollments.AddAsync(enrollment, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
