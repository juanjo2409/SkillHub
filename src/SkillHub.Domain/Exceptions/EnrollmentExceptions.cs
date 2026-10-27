using SkillHub.Domain.Errors;

namespace SkillHub.Domain.Exceptions;

public sealed class EnrollmentNotFoundException : DomainException
{
    public EnrollmentNotFoundException(string message = EnrollmentErrors.NotFound)
        : base(message)
    {
    }
}

public sealed class DuplicateEnrollmentException : DomainException
{
    public DuplicateEnrollmentException(string message = EnrollmentErrors.DuplicateActive)
        : base(message)
    {
    }
}

public sealed class CancellationNotAllowedException : DomainException
{
    public CancellationNotAllowedException(string message)
        : base(message)
    {
    }
}

public sealed class CertificateGenerationNotAllowedException : DomainException
{
    public CertificateGenerationNotAllowedException(string message = EnrollmentErrors.CertificateNotAllowedForCancelled)
        : base(message)
    {
    }
}

public sealed class UnauthorizedEnrollmentAccessException : DomainException
{
    public UnauthorizedEnrollmentAccessException(string message = EnrollmentErrors.UnauthorizedAccess)
        : base(message)
    {
    }
}
