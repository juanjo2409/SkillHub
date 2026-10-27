using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SkillHub.Application.DTOs.Auth;
using SkillHub.Application.Interface;
using SkillHub.Domain.Entities;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;
using SkillHub.Domain.ValueObjects;

namespace SkillHub.Infrastructure.Persistence.Repositories;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IParticipantRepository _participantRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<IdentityUser> userManager,
        IParticipantRepository participantRepository,
        IInstructorRepository instructorRepository,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _participantRepository = participantRepository;
        _instructorRepository = instructorRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterParticipantAsync(
        RegisterParticipantRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var email = new Email(request.Email);

        var existingUser = await _userManager.FindByEmailAsync(email.Value);
        if (existingUser is not null)
            throw new DomainValidationException(UserErrors.DuplicateEmail);

        var identityUser = new IdentityUser
        {
            UserName = email.Value,
            Email = email.Value,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new DomainValidationException(errors);
        }

        await _userManager.AddToRoleAsync(identityUser, "Participant");

        var existingParticipant = await _participantRepository.GetByEmailAsync(email.Value, cancellationToken);
        if (existingParticipant is not null)
        {
            existingParticipant.LinkIdentityUser(identityUser.Id);
            await _participantRepository.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var participant = new Participant(request.FullName, email, identityUser.Id);
            await _participantRepository.AddAsync(participant, cancellationToken);
            await _participantRepository.SaveChangesAsync(cancellationToken);
        }

        return GenerateJwtToken(identityUser, request.FullName.Trim(), "Participant");
    }

    public async Task<AuthResponseDto> LoginAsync(
        LoginRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var email = new Email(request.Email);
        var identityUser = await _userManager.FindByEmailAsync(email.Value)
            ?? throw new DomainValidationException("Invalid email or password.");

        var validPassword = await _userManager.CheckPasswordAsync(identityUser, request.Password);
        if (!validPassword)
            throw new DomainValidationException("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(identityUser);
        var role = roles.FirstOrDefault() ?? "Participant";

        var fullName = email.Value;
        if (role == "Employee")
        {
            var instructor = await _instructorRepository.GetByIdentityUserIdAsync(identityUser.Id, cancellationToken);
            if (instructor is not null)
                fullName = instructor.FullName;
        }
        else
        {
            var participant = await _participantRepository.GetByIdentityUserIdAsync(identityUser.Id, cancellationToken);
            if (participant is not null)
                fullName = participant.FullName;
        }

        return GenerateJwtToken(identityUser, fullName, role);
    }

    private AuthResponseDto GenerateJwtToken(IdentityUser user, string fullName, string role)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"]
            ?? "SkillHubSuperSecretJwtKeyForDevelopmentAndTesting2026!";
        var issuer = _configuration["JwtSettings:Issuer"] ?? "SkillHub.Api.Api";
        var audience = _configuration["JwtSettings:Audience"] ?? "SkillHub.Api.Client";
        var expiresAt = DateTime.UtcNow.AddHours(8);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new AuthResponseDto(tokenString, user.Email ?? string.Empty, fullName, role, expiresAt);
    }
}
