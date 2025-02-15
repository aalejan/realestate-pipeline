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

        public IndexModel(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<IndexModel> logger)
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
            var demoUser = await _userManager.FindByEmailAsync("johnDoe@gmail.com");
            if (demoUser != null)
            {
                await _signInManager.SignInAsync(demoUser, isPersistent: false);
                return RedirectToPage("/AgentDashboard");
            }

            ModelState.AddModelError(string.Empty, "Demo account not found.");
            return Page();
        }

        public async Task<IActionResult> OnPostDemoLoginClientAsync()
        {
            var demoUser = await _userManager.FindByEmailAsync("janeDoe@gmail.com");
            if (demoUser != null)
            {
                await _signInManager.SignInAsync(demoUser, isPersistent: false);
                return RedirectToPage("/ClientDashboard");
            }

            ModelState.AddModelError(string.Empty, "Demo account not found.");
            return Page();
        }
    }
}