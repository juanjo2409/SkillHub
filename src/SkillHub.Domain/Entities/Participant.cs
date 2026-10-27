using SkillHub.Domain.Abstractions;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Domain.Entities;

public class Participant : PersonBase
{
    public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

    protected Participant()
    {
    }

    public Participant(string fullName, Email email, string? identityUserId = null)
        : base(fullName, email, identityUserId)
    {
    }
}
