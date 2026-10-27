using SkillHub.Domain.Abstractions;
using SkillHub.Domain.Enums;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Domain.Entities;

public class Workshop : BaseEntity
{
    public WorkshopCode Code { get; private set; } = null!;
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateTimeRange Schedule { get; private set; } = null!;
    public int MaxCapacity { get; private set; }

    public Guid InstructorId { get; private set; }
    public Instructor Instructor { get; private set; } = null!;

    public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

    protected Workshop()
    {
    }

    public Workshop(
        WorkshopCode code,
        string title,
        string description,
        DateTimeRange schedule,
        int maxCapacity,
        Guid instructorId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException(WorkshopErrors.EmptyTitle);

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainValidationException(WorkshopErrors.EmptyDescription);

        if (!schedule.IsInFuture())
            throw new InvalidWorkshopDatesException(WorkshopErrors.PastDate);

        if (maxCapacity <= 0)
            throw new InvalidWorkshopCapacityException(WorkshopErrors.InvalidCapacity);

        Code = code;
        Title = title.Trim();
        Description = description.Trim();
        Schedule = schedule;
        MaxCapacity = maxCapacity;
        InstructorId = instructorId;
    }

    public int GetActiveEnrollmentsCount() =>
        Enrollments.Count(e => e.Status == EnrollmentStatus.Active);

    public int GetAvailableSpots() =>
        Math.Max(0, MaxCapacity - GetActiveEnrollmentsCount());

    public bool HasAvailableSpots() =>
        GetAvailableSpots() > 0;

    public bool HasStarted() =>
        DateTime.UtcNow >= Schedule.StartDate;

    // Enforces business rules 1, 2, 3, 4, and 6 (capacity, duplicates, schedule conflict, future workshop, re-enrollment)
    public Enrollment EnrollParticipant(Guid participantId, IEnumerable<DateTimeRange> activeParticipantSchedules)
    {
        if (HasStarted())
            throw new WorkshopAlreadyStartedException();

        var existingEnrollment = Enrollments.FirstOrDefault(e => e.ParticipantId == participantId);

        if (existingEnrollment is { Status: EnrollmentStatus.Active })
            throw new DuplicateEnrollmentException();

        if (!HasAvailableSpots())
            throw new WorkshopCapacityExceededException();

        if (activeParticipantSchedules.Any(otherSchedule => Schedule.OverlapsWith(otherSchedule)))
            throw new WorkshopScheduleConflictException();

        if (existingEnrollment is { Status: EnrollmentStatus.Cancelled })
        {
            existingEnrollment.Reactivate(Schedule.StartDate);
            return existingEnrollment;
        }

        var enrollment = new Enrollment(participantId, Id);
        Enrollments.Add(enrollment);
        return enrollment;
    }
}
