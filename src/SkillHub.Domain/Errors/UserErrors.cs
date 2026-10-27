namespace SkillHub.Domain.Errors;

public static class UserErrors
{
    public const string EmptyFullName = "Full name is required.";
    public const string EmptySpecialty = "Instructor specialty is required.";
    public const string EmptyEmail = "Email address is required.";
    public const string InvalidEmailFormat = "The provided email address format is invalid.";
    public const string DuplicateEmail = "A user with this email address is already registered.";
    public const string InstructorNotFound = "No registered instructor was found with the specified email.";
    public const string ParticipantNotFound = "No registered participant was found with the specified email.";
}
