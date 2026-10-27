using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Application.DTOs.Workshops;
using SkillHub.Application.Interface;

namespace SkillHub.Api.Controllers;

[ApiController]
[Route("api/v1/reports")]
[Authorize(Roles = "Employee")]
public class ReportsController : ControllerBase
{
    private readonly IWorkshopService _workshopService;

    public ReportsController(IWorkshopService workshopService)
    {
        _workshopService = workshopService;
    }

    [HttpGet("top-workshops")]
    public async Task<ActionResult<IReadOnlyList<TopWorkshopResponseDto>>> GetTopWorkshops(
        CancellationToken cancellationToken)
    {
        var report = await _workshopService.GetTopWorkshopsAsync(cancellationToken);
        return Ok(report);
    }
}
