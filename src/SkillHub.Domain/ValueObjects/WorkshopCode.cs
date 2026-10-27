using System.Text.RegularExpressions;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;

namespace SkillHub.Domain.ValueObjects;

public sealed record WorkshopCode
{
    // Matches codes like FOT-001, COC-102, ORA-01
    private static readonly Regex CodeRegex = new(
        @"^[A-Z]{2,5}-\d{2,5}$",
        RegexOptions.Compiled);

    public string Value { get; init; } = string.Empty;

    private WorkshopCode()
    {
    }

    public WorkshopCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidWorkshopCodeException(WorkshopErrors.EmptyCode);

        var normalized = value.Trim().ToUpperInvariant();

        if (!CodeRegex.IsMatch(normalized))
            throw new InvalidWorkshopCodeException(WorkshopErrors.InvalidCodeFormat);

        Value = normalized;
    }

    public static implicit operator string(WorkshopCode code) => code.Value;
    public static implicit operator WorkshopCode(string value) => new(value);

    public override string ToString() => Value;
}
