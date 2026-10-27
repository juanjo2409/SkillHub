using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Application.Interface;
using SkillHub.Domain.Exceptions;
using SkillHub.Web.Models;

namespace SkillHub.Web.Controllers;

public class WorkshopsController : Controller
{
    private readonly IWorkshopService _workshopService;
    private readonly IEnrollmentService _enrollmentService;

    public WorkshopsController(
        IWorkshopService workshopService,
        IEnrollmentService enrollmentService)
    {
        _workshopService = workshopService;
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var workshops = await _workshopService.GetUpcomingCatalogAsync(cancellationToken);
        var enrolledIds = new HashSet<Guid>();

        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Participant"))
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    var myEnrollments = await _enrollmentService.GetMyEnrollmentsAsync(userId, cancellationToken);
                    enrolledIds = myEnrollments
                        .Where(e => e.Status == "Active")
                        .Select(e => e.WorkshopId)
                        .ToHashSet();
                }
                catch (DomainException)
                {
                }
            }
        }

        var viewModel = new WorkshopCatalogViewModel
        {
            Workshops = workshops,
            ActiveEnrolledWorkshopIds = enrolledIds
        };

        return View(viewModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var workshop = await _workshopService.GetByIdAsync(id, cancellationToken);
            return View(workshop);
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> ExportEnrollments(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var (fileBytes, fileName) = await _workshopService.ExportWorkshopEnrollmentsExcelAsync(id, cancellationToken);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Profile", "Account");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
