using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;

namespace SkillHub.Domain.ValueObjects;

public sealed record DateTimeRange
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    private DateTimeRange()
    {
    }

    public DateTimeRange(DateTime startDate, DateTime endDate)
    {
        var normalizedStart = NormalizeToUtc(startDate);
        var normalizedEnd = NormalizeToUtc(endDate);

        if (normalizedEnd <= normalizedStart)
            throw new InvalidWorkshopDatesException(WorkshopErrors.InvalidDates);

        StartDate = normalizedStart;
        EndDate = normalizedEnd;
    }

    public bool OverlapsWith(DateTimeRange other) =>
        StartDate < other.EndDate && EndDate > other.StartDate;

    public bool IsInFuture() =>
        StartDate > DateTime.UtcNow;

    private static DateTime NormalizeToUtc(DateTime dateTime) =>
        dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc)
        };
}
