using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.Interface;
using SkillHub.Domain.Errors;
using SkillHub.Domain.Exceptions;

namespace SkillHub.Api.Controllers;

[ApiController]
[Route("api/v1/enrollments")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<ActionResult<EnrollmentResponseDto>> Enroll(
        [FromBody] CreateEnrollmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var enrollment = await _enrollmentService.EnrollParticipantAsync(userId, request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, enrollment);
    }

    [HttpGet("mine")]
    [Authorize(Roles = "Participant")]
    public async Task<ActionResult<IReadOnlyList<EnrollmentResponseDto>>> GetMine(
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var enrollments = await _enrollmentService.GetMyEnrollmentsAsync(userId, cancellationToken);
        return Ok(enrollments);
    }

    [HttpPatch("{id:guid}/cancel")]
    [Authorize(Roles = "Participant,Employee")]
    public async Task<ActionResult<EnrollmentResponseDto>> Cancel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var isEmployee = User.IsInRole("Employee");
        var cancelled = await _enrollmentService.CancelEnrollmentAsync(id, userId, isEmployee, cancellationToken);
        return Ok(cancelled);
    }

    [HttpPost("import")]
    [Authorize(Roles = "Employee")]
    public async Task<ActionResult<ImportEnrollmentResponseDto>> ImportFromExcel(
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            throw new DomainValidationException(EnrollmentErrors.EmptyImportFile);

        await using var stream = file.OpenReadStream();
        var result = await _enrollmentService.ImportFromExcelAsync(stream, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/certificate")]
    [Authorize(Roles = "Participant,Employee")]
    public async Task<IActionResult> DownloadCertificate(
        Guid id,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var isEmployee = User.IsInRole("Employee");
        var pdfBytes = await _enrollmentService.GenerateCertificatePdfAsync(id, userId, isEmployee, cancellationToken);
        return File(pdfBytes, "application/pdf", $"certificate-{id}.pdf");
    }

    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
}
