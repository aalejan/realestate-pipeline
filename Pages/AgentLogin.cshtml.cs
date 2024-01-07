using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;
using System.ComponentModel.DataAnnotations;

namespace RealEstatePipeline.Pages
{
    public class AgentLoginModel : PageModel
    {

        private readonly SignInManager<ApplicationUser> _signInManager;

        public AgentLoginModel(SignInManager<ApplicationUser> signInManager) {

            _signInManager = signInManager;
        }

        [BindProperty]
        public UserLogin Login { get; set; }
        public class UserLogin
        {
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Login.Email, Login.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToPage("AgentDashboard"); // Redirect to the homepage or dashboard
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            return Page(); // If validation fails, redisplay the form
        }
    }
}
