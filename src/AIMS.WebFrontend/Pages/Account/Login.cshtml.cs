using AIMS.Infrastructure.Data;
using AIMS.Infrastructure.IdentityClass;
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

        public LoginModel(AppDbContext appDbContext, SignInManager<ApplicationUser> singInManager , UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _singInManager = singInManager;
            _userManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                Redirect("Index");
                var user= _userManager.Users.Where(x => x.UserName == LoginInput.UserName).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("Default","Error");
                    return Page();
                }
                var signin = await _singInManager.CheckPasswordSignInAsync(user, LoginInput.Password, false);
                if (signin.Succeeded)
                {
                    await _singInManager.SignInAsync(user,false);
                    return Redirect("/");


                }
                return Page();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
            // Do something
        }
        [BindProperty]
        public LoginViewModel LoginInput { get; set; }

    }

    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
