namespace SkillHub.Application.DTOs.Auth;

public record RegisterParticipantRequestDto(
    string FullName,
    string Email,
    string Password);
