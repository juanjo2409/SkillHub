using Microsoft.EntityFrameworkCore;
using SkillHub.Application.Interface;
using SkillHub.Domain.Entities;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Infrastructure.Persistence.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly ApplicationDbContext _context;

    public ParticipantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Participant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Participants.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<Participant?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var emailVo = new Email(email);
        return await _context.Participants.FirstOrDefaultAsync(p => p.Email == emailVo, cancellationToken);
    }

    public async Task<Participant?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default) =>
        await _context.Participants.FirstOrDefaultAsync(p => p.IdentityUserId == identityUserId, cancellationToken);

    public async Task AddAsync(Participant participant, CancellationToken cancellationToken = default) =>
        await _context.Participants.AddAsync(participant, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
