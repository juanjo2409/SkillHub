using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.Interface;
using SkillHub.Domain.Exceptions;

namespace SkillHub.Web.Controllers;

[Authorize]
public class EnrollmentsController : Controller
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Mine(CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            var enrollments = await _enrollmentService.GetMyEnrollmentsAsync(userId, cancellationToken);
            return View(enrollments);
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Workshops");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Guid workshopId, CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            var enrollment = await _enrollmentService.EnrollParticipantAsync(
                userId,
                new CreateEnrollmentRequestDto(workshopId),
                cancellationToken);

            TempData["SuccessMessage"] = $"Te has inscrito exitosamente en el taller '{enrollment.WorkshopTitle}' ({enrollment.WorkshopCode}).";
            return RedirectToAction(nameof(Mine));
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Details", "Workshops", new { id = workshopId });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(Guid id, Guid? workshopId, CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isEmployee = User.IsInRole("Employee");
            var cancelled = await _enrollmentService.CancelEnrollmentAsync(id, userId, isEmployee, cancellationToken);

            TempData["SuccessMessage"] = $"La inscripción al taller '{cancelled.WorkshopTitle}' ha sido cancelada y el cupo fue liberado.";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        if (workshopId.HasValue && User.IsInRole("Employee"))
        {
            return RedirectToAction("Enrollments", "Workshops", new { id = workshopId.Value });
        }

        return RedirectToAction(nameof(Mine));
    }

    [HttpPost]
    [Authorize(Roles = "Employee")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Import(IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            TempData["ErrorMessage"] = "Debes seleccionar un archivo Excel (.xlsx) válido.";
            return RedirectToAction("Profile", "Account");
        }

        try
        {
            await using var stream = file.OpenReadStream();
            var result = await _enrollmentService.ImportFromExcelAsync(stream, cancellationToken);
            TempData["SuccessMessage"] = $"Importación completada: {result.TotalEnrolled} inscritos exitosamente y {result.RejectedRows.Count} filas rechazadas.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al procesar el archivo Excel: {ex.Message}";
        }

        return RedirectToAction("Profile", "Account");
    }

    [HttpGet]
    public async Task<IActionResult> Certificate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var userId = GetCurrentUserId();
            var isEmployee = User.IsInRole("Employee");
            var pdfBytes = await _enrollmentService.GenerateCertificatePdfAsync(id, userId, isEmployee, cancellationToken);
            return File(pdfBytes, "application/pdf", $"Constancia-SkillHub-{id}.pdf");
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Mine));
        }
    }

    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
}
