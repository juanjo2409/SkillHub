using SkillHub.Domain.Abstractions;
using SkillHub.Domain.Enums;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;

namespace SkillHub.Domain.Entities;

public class Enrollment : BaseEntity
{
    public Guid ParticipantId { get; private set; }
    public Participant Participant { get; private set; } = null!;

    public Guid WorkshopId { get; private set; }
    public Workshop Workshop { get; private set; } = null!;

    public DateTime EnrollmentDate { get; private set; }
    public EnrollmentStatus Status { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public string? CancelledByUserId { get; private set; }

    protected Enrollment()
    {
    }

    public Enrollment(Guid participantId, Guid workshopId)
    {
        ParticipantId = participantId;
        WorkshopId = workshopId;
        EnrollmentDate = DateTime.UtcNow;
        Status = EnrollmentStatus.Active;
    }

    // Rule 5: Participants can cancel up to 24 hours before workshop starts
    public void CancelByParticipant(string userId, DateTime workshopStartDate)
    {
        if (Status == EnrollmentStatus.Cancelled)
            throw new CancellationNotAllowedException(EnrollmentErrors.AlreadyCancelled);

        if (DateTime.UtcNow.AddHours(24) > workshopStartDate)
            throw new CancellationNotAllowedException(EnrollmentErrors.ParticipantCancellationExpired);

        Status = EnrollmentStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancelledByUserId = userId;
    }

    // Rule 5: Employees can cancel up until the workshop starts
    public void CancelByEmployee(string employeeUserId, DateTime workshopStartDate)
    {
        if (Status == EnrollmentStatus.Cancelled)
            throw new CancellationNotAllowedException(EnrollmentErrors.AlreadyCancelled);

        if (DateTime.UtcNow >= workshopStartDate)
            throw new CancellationNotAllowedException(EnrollmentErrors.EmployeeCancellationStarted);

        Status = EnrollmentStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        CancelledByUserId = employeeUserId;
    }

    // Rule 6: Re-enrollment of a previously cancelled enrollment
    public void Reactivate(DateTime workshopStartDate)
    {
        if (DateTime.UtcNow >= workshopStartDate)
            throw new WorkshopAlreadyStartedException();

        Status = EnrollmentStatus.Active;
        EnrollmentDate = DateTime.UtcNow;
        CancelledAt = null;
        CancelledByUserId = null;
    }

    // Certificate rule: Only active enrollments can generate a PDF certificate
    public void EnsureCanGenerateCertificate()
    {
        if (Status != EnrollmentStatus.Active)
            throw new CertificateGenerationNotAllowedException();
    }
}
