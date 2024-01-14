using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RealEstatePipeline.Pages
{
    public class ClientDashboardModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AgentRegistrationModel> _logger;


        public bool IsClient {  get; set; }
        public string UserId {  get; set; }
        public List<string> PrimaryLanguages { get; private set; }


        public ClientRegistration Client {  get; set; }

        public ClientDashboardModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AgentRegistrationModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index"); // Redirect to the home page or login page
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


        public async Task<IActionResult> OnGetAsync()
        {
           
                var user = await _userManager.GetUserAsync(User) as ClientRegistration;
                if (user != null)
                {
                    IsClient = await _userManager.IsInRoleAsync(user, "Client");
                    UserId = user.Id;
                    

                Client = new ClientRegistration
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    MaximumBudget = user.MaximumBudget,
                    MinimumBudget = user.MinimumBudget,
                    ProfileDescription = user.ProfileDescription,
                    PrimaryLanguage = user.PrimaryLanguage,
                    LocationPreference = user.LocationPreference,
                    PreferredCommunicationMethod = user.PreferredCommunicationMethod,
                    PropertyTypes = user.PropertyTypes

                };
                }
            return Page();
           
        }


    }

}
