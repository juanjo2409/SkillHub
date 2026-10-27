using SkillHub.Domain.Abstractions;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Domain.Entities;

public class Instructor : PersonBase
{
    public string Specialty { get; private set; } = string.Empty;

    public ICollection<Workshop> Workshops { get; private set; } = new List<Workshop>();

    protected Instructor()
    {
    }

    public Instructor(string fullName, Email email, string specialty, string? identityUserId = null)
        : base(fullName, email, identityUserId)
    {
        if (string.IsNullOrWhiteSpace(specialty))
            throw new DomainValidationException(UserErrors.EmptySpecialty);

        Specialty = specialty.Trim();
    }
}
