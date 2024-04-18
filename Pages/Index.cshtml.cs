using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;

namespace RealEstatePipeline.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<IndexModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostDemoLoginAsync()
        {
            // Assuming "demo@example.com" is the email of your pre-created demo agent
            var demoUser = await _userManager.FindByEmailAsync("johnDoe@gmail.com");
            if (demoUser != null)
            {
                // Sign in the demo user
                await _signInManager.SignInAsync(demoUser, isPersistent: false);
                return RedirectToPage("/AgentDashboard");  // Redirect to the agent dashboard or another appropriate page
            }
            else
            {
                // Handle the case where the demo user does not exist
                ModelState.AddModelError(string.Empty, "Demo account not found.");
                return Page();
            }
        }
    }
}
