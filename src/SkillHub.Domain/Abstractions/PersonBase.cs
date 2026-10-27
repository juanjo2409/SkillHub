using SkillHub.Domain.Abstractions.Interfaces;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Domain.Abstractions;

public abstract class PersonBase : BaseEntity, IPerson
{
    public string FullName { get; protected set; } = string.Empty;
    public Email Email { get; protected set; } = null!;
    public string? IdentityUserId { get; protected set; }

    protected PersonBase()
    {
    }

    protected PersonBase(string fullName, Email email, string? identityUserId = null)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainValidationException(UserErrors.EmptyFullName);

        FullName = fullName.Trim();
        Email = email;
        IdentityUserId = identityUserId;
    }

    public void LinkIdentityUser(string identityUserId)
    {
        IdentityUserId = identityUserId.Trim();
    }
}
