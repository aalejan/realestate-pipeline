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

        public Agent_Info Agent { get; private set; }

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
            var user = await _userManager.GetUserAsync(User) as Agent_Info;
            if (user != null)
            {
                IsAgent = await _userManager.IsInRoleAsync(user, "Agent");
                UserId = user.Id; // Store the user ID

                Agent = new Agent_Info
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfileDescription = user.ProfileDescription,
                    PrimaryLanguage = user.PrimaryLanguage,
                    PropertyTypes = user.PropertyTypes,
                    PreferredCommunicationMethod = user.PreferredCommunicationMethod,
                    LocationPreference = user.LocationPreference,
                    YearsOfExperience = user.YearsOfExperience,
                    Ratings = user.Ratings,
                    Email = user.Email,


                };
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
