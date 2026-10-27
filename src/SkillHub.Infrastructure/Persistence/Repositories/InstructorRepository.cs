using Microsoft.EntityFrameworkCore;
using SkillHub.Application.Interface;
using SkillHub.Domain.Entities;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Infrastructure.Persistence.Repositories;

public class InstructorRepository : IInstructorRepository
{
    private readonly ApplicationDbContext _context;

    public InstructorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Instructor?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Instructors.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

    public async Task<Instructor?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var emailVo = new Email(email);
        return await _context.Instructors.FirstOrDefaultAsync(i => i.Email == emailVo, cancellationToken);
    }

    public async Task<Instructor?> GetByIdentityUserIdAsync(string identityUserId, CancellationToken cancellationToken = default) =>
        await _context.Instructors.FirstOrDefaultAsync(i => i.IdentityUserId == identityUserId, cancellationToken);

    public async Task AddAsync(Instructor instructor, CancellationToken cancellationToken = default) =>
        await _context.Instructors.AddAsync(instructor, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
