using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;

namespace RealEstatePipeline.Pages
{
    public class ClientDashboardModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        
        public ClientDashboardModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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


        public void OnGet()
        {
        }
    }
}
