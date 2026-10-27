using System.Text.RegularExpressions;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;

namespace SkillHub.Domain.ValueObjects;

public sealed record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; init; } = string.Empty;

    private Email()
    {
    }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException(UserErrors.EmptyEmail);

        var normalized = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalized))
            throw new InvalidEmailException(UserErrors.InvalidEmailFormat);

        Value = normalized;
    }

    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new(value);

    public override string ToString() => Value;
}
