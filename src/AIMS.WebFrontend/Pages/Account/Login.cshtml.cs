using AIMS.Core.Entities;
using AIMS.Infrastructure.Data;
using AIMS.Infrastructure.IdentityClass;
using AIMS.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AIMS.WebFrontend.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _appDbContext;
        private readonly SignInManager<ApplicationUser> _singInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IActivityLogger _activityLogger;

        public LoginModel(
            AppDbContext appDbContext, 
            SignInManager<ApplicationUser> singInManager, 
            UserManager<ApplicationUser> userManager,
            IActivityLogger activityLogger)
        {
            _appDbContext = appDbContext;
            _singInManager = singInManager;
            _userManager = userManager;
            _activityLogger = activityLogger;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var user = _userManager.Users.Where(x => x.UserName == LoginInput.UserName).FirstOrDefault();
                if (user == null)
                {
                    // Log failed login attempt - user not found
                    await _activityLogger.LogSecurityActivityAsync(
                        ActivityType.LoginFailed,
                        $"Login attempt with unknown username: {LoginInput.UserName}",
                        "Failed",
                        null,
                        LoginInput.UserName);

                    ModelState.AddModelError("Default", "Invalid username or password");
                    return Page();
                }

                var signin = await _singInManager.CheckPasswordSignInAsync(user, LoginInput.Password, false);
                if (signin.Succeeded)
                {
                    await _singInManager.SignInAsync(user, false);

                    // Log successful login
                    await _activityLogger.LogSecurityActivityAsync(
                        ActivityType.Login,
                        $"User '{user.UserName}' logged in successfully",
                        "Success",
                        user.Id,
                        user.UserName);

                    return Redirect("/");
                }

                // Log failed login attempt - wrong password
                await _activityLogger.LogSecurityActivityAsync(
                    ActivityType.LoginFailed,
                    $"Failed login attempt for user: {user.UserName} (invalid password)",
                    "Failed",
                    user.Id,
                    user.UserName);

                ModelState.AddModelError("Default", "Invalid username or password");
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [BindProperty]
        public LoginViewModel LoginInput { get; set; } = new LoginViewModel();
    }

    public class LoginViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
