using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.Application.DTOs.Auth;
using SkillHub.Application.DTOs.Enrollments;
using SkillHub.Application.DTOs.Workshops;
using SkillHub.Application.Interface;
using SkillHub.Domain.Exceptions;
using SkillHub.Web.Models;

namespace SkillHub.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IWorkshopService _workshopService;
    private readonly IEnrollmentService _enrollmentService;

    public AccountController(
        IAuthService authService,
        IWorkshopService workshopService,
        IEnrollmentService enrollmentService)
    {
        _authService = authService;
        _workshopService = workshopService;
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var fullName = User.Identity?.Name ?? "Usuario";
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var role = User.IsInRole("Employee") ? "Employee" : "Participant";

        IReadOnlyList<EnrollmentResponseDto> myEnrollments = Array.Empty<EnrollmentResponseDto>();
        IReadOnlyList<WorkshopResponseDto> myWorkshops = Array.Empty<WorkshopResponseDto>();
        IReadOnlyList<WorkshopResponseDto> allWorkshops = Array.Empty<WorkshopResponseDto>();
        IReadOnlyList<TopWorkshopResponseDto> topWorkshops = Array.Empty<TopWorkshopResponseDto>();
        var enrollmentsByWorkshop = new Dictionary<Guid, IReadOnlyList<EnrollmentResponseDto>>();

        if (role == "Employee")
        {
            allWorkshops = await _workshopService.GetUpcomingCatalogAsync(cancellationToken);
            topWorkshops = await _workshopService.GetTopWorkshopsAsync(cancellationToken);
            myWorkshops = allWorkshops
                .Where(w => string.Equals(w.InstructorEmail, email, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var workshop in myWorkshops)
            {
                var workshopEnrollments = await _workshopService.GetWorkshopEnrollmentsAsync(workshop.Id, cancellationToken);
                enrollmentsByWorkshop[workshop.Id] = workshopEnrollments;
            }
        }
        else
        {
            try
            {
                myEnrollments = await _enrollmentService.GetMyEnrollmentsAsync(userId, cancellationToken);
            }
            catch (DomainException)
            {
            }
        }

        var profileVm = new UserProfileViewModel
        {
            FullName = fullName,
            Email = email,
            Role = role,
            MyEnrollments = myEnrollments,
            MyInstructorWorkshops = myWorkshops,
            EnrollmentsByWorkshop = enrollmentsByWorkshop,
            AllWorkshops = allWorkshops,
            TopWorkshops = topWorkshops
        };

        return View(profileVm);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Workshops");

        return View(new LoginViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var response = await _authService.LoginAsync(
                new LoginRequestDto(model.Email, model.Password),
                cancellationToken);

            await SignInWithCookieAsync(response);
            TempData["SuccessMessage"] = $"Bienvenido de nuevo, {response.FullName}.";
            return RedirectToAction("Profile", "Account");
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Workshops");

        return View(new RegisterViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var response = await _authService.RegisterParticipantAsync(
                new RegisterParticipantRequestDto(model.FullName, model.Email, model.Password),
                cancellationToken);

            await SignInWithCookieAsync(response);
            TempData["SuccessMessage"] = $"Cuenta creada exitosamente. ¡Bienvenido a SkillHub, {response.FullName}!";
            return RedirectToAction("Profile", "Account");
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["SuccessMessage"] = "Has cerrado sesión correctamente.";
        return RedirectToAction("Index", "Workshops");
    }

    private async Task SignInWithCookieAsync(AuthResponseDto authResponse)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(authResponse.Token);
        var userId = jwt.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.NameIdentifier || c.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, authResponse.FullName),
            new(ClaimTypes.Email, authResponse.Email),
            new(ClaimTypes.Role, authResponse.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                AllowRefresh = true
            });
    }
}
