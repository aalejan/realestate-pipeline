using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;

namespace RealEstatePipeline.Pages
{
    public class AgentDashboardModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public bool IsAgent { get; private set; }
        public string UserId { get; private set; } // Property to store user ID

        public AgentDashboardModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index"); // Redirect to the home page or login page
        }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                IsAgent = await _userManager.IsInRoleAsync(user, "Agent");
                UserId = user.Id; // Store the user ID
            }
        }
        public bool IsUserLoggedIn()
        {
            return User.Identity.IsAuthenticated;
        }

        public string GetUserName()
        {
            return User.Identity.Name; // Gets the user's username
        }

        public string GetUserEmail()
        {
            var user = _userManager.GetUserAsync(User).Result;
            return user?.Email; // Gets the user's email
        }

        
        public string GetBio()
        {
            var user = _userManager.GetUserAsync(User).Result;
            return user?.FirstName;
        }
        
        
    }
}
