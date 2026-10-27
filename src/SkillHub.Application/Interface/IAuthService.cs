using SkillHub.Application.DTOs.Auth;

namespace SkillHub.Application.Interface;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterParticipantAsync(RegisterParticipantRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<(string FullName, string Email, string Role, string? Specialty)> GetUserProfileAsync(string identityUserId, CancellationToken cancellationToken = default);
    Task UpdateUserProfileAsync(string identityUserId, string fullName, string? specialty, string? currentPassword, string? newPassword, CancellationToken cancellationToken = default);
}
