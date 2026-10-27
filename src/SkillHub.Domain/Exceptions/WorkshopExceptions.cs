using SkillHub.Domain.Errors;

namespace SkillHub.Domain.Exceptions;

public sealed class InvalidWorkshopCodeException : DomainException
{
    public InvalidWorkshopCodeException(string message = WorkshopErrors.InvalidCodeFormat)
        : base(message)
    {
    }
}

public sealed class DuplicateWorkshopCodeException : DomainException
{
    public DuplicateWorkshopCodeException(string message = WorkshopErrors.DuplicateCode)
        : base(message)
    {
    }
}

public sealed class WorkshopNotFoundException : DomainException
{
    public WorkshopNotFoundException(string message = WorkshopErrors.NotFound)
        : base(message)
    {
    }
}

public sealed class WorkshopCapacityExceededException : DomainException
{
    public WorkshopCapacityExceededException(string message = WorkshopErrors.CapacityExceeded)
        : base(message)
    {
    }
}

public sealed class WorkshopAlreadyStartedException : DomainException
{
    public WorkshopAlreadyStartedException(string message = WorkshopErrors.AlreadyStarted)
        : base(message)
    {
    }
}

public sealed class WorkshopScheduleConflictException : DomainException
{
    public WorkshopScheduleConflictException(string message = WorkshopErrors.ScheduleConflict)
        : base(message)
    {
    }
}

public sealed class InvalidWorkshopDatesException : DomainException
{
    public InvalidWorkshopDatesException(string message = WorkshopErrors.InvalidDates)
        : base(message)
    {
    }
}

public sealed class InvalidWorkshopCapacityException : DomainException
{
    public InvalidWorkshopCapacityException(string message = WorkshopErrors.InvalidCapacity)
        : base(message)
    {
    }
}
