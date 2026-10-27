using SkillHub.Domain.Errors;

namespace SkillHub.Domain.Exceptions;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException(string message = UserErrors.InvalidEmailFormat)
        : base(message)
    {
    }
}

public sealed class InstructorNotFoundException : DomainException
{
    public InstructorNotFoundException(string message = UserErrors.InstructorNotFound)
        : base(message)
    {
    }
}

public sealed class ParticipantNotFoundException : DomainException
{
    public ParticipantNotFoundException(string message = UserErrors.ParticipantNotFound)
        : base(message)
    {
    }
}
