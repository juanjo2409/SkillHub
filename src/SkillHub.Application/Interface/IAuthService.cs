using SkillHub.Application.DTOs.Auth;

namespace SkillHub.Application.Interface;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterParticipantAsync(RegisterParticipantRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
}
