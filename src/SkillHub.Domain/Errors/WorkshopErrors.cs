namespace SkillHub.Domain.Errors;

public static class WorkshopErrors
{
    public const string EmptyCode = "Workshop code is required.";
    public const string InvalidCodeFormat = "Workshop code must follow the format 'AAA-001' (e.g., FOT-001).";
    public const string DuplicateCode = "A workshop with the same code already exists.";
    public const string EmptyTitle = "Workshop title is required.";
    public const string EmptyDescription = "Workshop description is required.";
    public const string CapacityExceeded = "The workshop has reached its maximum capacity.";
    public const string AlreadyStarted = "Cannot enroll in a workshop that has already started.";
    public const string ScheduleConflict = "The participant already has an active enrollment that overlaps with this workshop's schedule.";
    public const string InvalidDates = "The workshop end date must be greater than the start date.";
    public const string PastDate = "The workshop must be created with a future start date.";
    public const string InvalidCapacity = "The maximum capacity must be greater than zero.";
    public const string NotFound = "The requested workshop was not found.";
}
