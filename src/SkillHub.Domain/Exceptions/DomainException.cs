namespace SkillHub.Domain.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}

public sealed class DomainValidationException : DomainException
{
    public DomainValidationException(string message) : base(message)
    {
    }
}
