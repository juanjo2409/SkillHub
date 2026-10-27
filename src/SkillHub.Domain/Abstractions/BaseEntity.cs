using SkillHub.Domain.Abstractions.Interfaces;

namespace SkillHub.Domain.Abstractions;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}
