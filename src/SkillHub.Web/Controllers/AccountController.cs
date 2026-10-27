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

        try
        {
            var profile = await _authService.GetUserProfileAsync(userId, cancellationToken);
            return View(new UserProfileViewModel
            {
                FullName = profile.FullName,
                Email = profile.Email,
                Role = profile.Role,
                Specialty = profile.Specialty
            });
        }
        catch (DomainException)
        {
            return View(new UserProfileViewModel
            {
                FullName = User.Identity?.Name ?? "Usuario",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
                Role = User.IsInRole("Employee") ? "Employee" : "Participant"
            });
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(UserProfileViewModel model, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        var role = User.IsInRole("Employee") ? "Employee" : "Participant";

        model.Email = email;
        model.Role = role;

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _authService.UpdateUserProfileAsync(
                userId,
                model.FullName,
                model.Specialty,
                model.CurrentPassword,
                model.NewPassword,
                cancellationToken);

            await RefreshUserCookieAsync(userId, model.FullName.Trim(), email, role);
            TempData["SuccessMessage"] = "Los datos de tu perfil se actualizaron correctamente.";
            return RedirectToAction(nameof(Profile));
        }
        catch (DomainException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    [Authorize(Roles = "Employee")]
    public async Task<IActionResult> Students(CancellationToken cancellationToken)
    {
        var fullName = User.Identity?.Name ?? "Instructor";
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        var allWorkshops = await _workshopService.GetUpcomingCatalogAsync(cancellationToken);
        var topWorkshops = await _workshopService.GetTopWorkshopsAsync(cancellationToken);
        var myWorkshops = allWorkshops
            .Where(w => string.Equals(w.InstructorEmail, email, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var enrollmentsByWorkshop = new Dictionary<Guid, IReadOnlyList<EnrollmentResponseDto>>();
        foreach (var workshop in myWorkshops)
        {
            var workshopEnrollments = await _workshopService.GetWorkshopEnrollmentsAsync(workshop.Id, cancellationToken);
            enrollmentsByWorkshop[workshop.Id] = workshopEnrollments;
        }

        var vm = new UserProfileViewModel
        {
            FullName = fullName,
            Email = email,
            Role = "Employee",
            MyInstructorWorkshops = myWorkshops,
            EnrollmentsByWorkshop = enrollmentsByWorkshop,
            AllWorkshops = allWorkshops,
            TopWorkshops = topWorkshops
        };

        return View(vm);
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
            return response.Role == "Employee"
                ? RedirectToAction(nameof(Students))
                : RedirectToAction("Index", "Workshops");
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
            return RedirectToAction("Index", "Workshops");
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

        await RefreshUserCookieAsync(userId, authResponse.FullName, authResponse.Email, authResponse.Role);
    }

    private async Task RefreshUserCookieAsync(string userId, string fullName, string email, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, fullName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
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
