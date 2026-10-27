using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;
using SkillHub.Application.Interface;

namespace SkillHub.Api.Controllers;

[ApiController]
[Route("api/v1/workshops")]
public class WorkshopsController : ControllerBase
{
    private readonly IWorkshopService _workshopService;

    public WorkshopsController(IWorkshopService workshopService)
    {
        _workshopService = workshopService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyList<WorkshopResponseDto>>> GetUpcomingCatalog(
        CancellationToken cancellationToken)
    {
        var workshops = await _workshopService.GetUpcomingCatalogAsync(cancellationToken);
        return Ok(workshops);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<WorkshopResponseDto>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var workshop = await _workshopService.GetByIdAsync(id, cancellationToken);
        return Ok(workshop);
    }

    [HttpPost]
    [Authorize(Roles = "Employee")]
    public async Task<ActionResult<WorkshopResponseDto>> Create(
        [FromBody] CreateWorkshopRequestDto request,
        CancellationToken cancellationToken)
    {
        var created = await _workshopService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:guid}/enrollments")]
    [Authorize(Roles = "Employee")]
    public async Task<ActionResult<IReadOnlyList<EnrollmentResponseDto>>> GetWorkshopEnrollments(
        Guid id,
        CancellationToken cancellationToken)
    {
        var enrollments = await _workshopService.GetWorkshopEnrollmentsAsync(id, cancellationToken);
        return Ok(enrollments);
    }

    [HttpGet("{id:guid}/enrollments/export")]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> ExportWorkshopEnrollmentsExcel(
        Guid id,
        CancellationToken cancellationToken)
    {
        var (fileBytes, fileName) = await _workshopService.ExportWorkshopEnrollmentsExcelAsync(id, cancellationToken);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
