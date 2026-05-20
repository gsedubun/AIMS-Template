using System.Security.Claims;
using AIMS.Core.Entities;
using AIMS.Infrastructure.IdentityClass;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AIMS.WebFrontend.Pages.Account;

public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IActivityLogger _activityLogger;

    public LogoutModel(SignInManager<ApplicationUser> signInManager, IActivityLogger activityLogger)
    {
        _signInManager = signInManager;
        _activityLogger = activityLogger;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        // Capture user info before signing out
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.Identity?.Name;

        // Log logout activity before signing out
        await _activityLogger.LogSecurityActivityAsync(
            ActivityType.Logout,
            $"User '{userName}' logged out",
            "Success",
            userId,
            userName);

        await _signInManager.SignOutAsync();

        return RedirectToPage("/Account/Login");
    }
}
