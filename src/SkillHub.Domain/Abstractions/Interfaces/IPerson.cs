using SkillHub.Domain.ValueObjects;

namespace SkillHub.Domain.Abstractions.Interfaces;

public interface IPerson : IEntity
{
    string FullName { get; }
    Email Email { get; }
    string? IdentityUserId { get; }
    void LinkIdentityUser(string identityUserId);
}
