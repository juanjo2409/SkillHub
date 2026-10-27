namespace SkillHub.Domain.Errors;

public static class EnrollmentErrors
{
    public const string DuplicateActive = "The participant already has an active enrollment in this workshop.";
    public const string AlreadyCancelled = "The enrollment is already cancelled.";
    public const string ParticipantCancellationExpired = "Participants can only cancel their enrollment up to 24 hours before the workshop starts.";
    public const string EmployeeCancellationStarted = "Employees cannot cancel an enrollment once the workshop has started.";
    public const string CertificateNotAllowedForCancelled = "A certificate can only be generated for active enrollments.";
    public const string UnauthorizedAccess = "Participants can only view and manage their own enrollments.";
    public const string EmptyImportFile = "The uploaded Excel file is empty or invalid.";
    public const string NotFound = "The requested enrollment was not found.";
}
