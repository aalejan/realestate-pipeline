using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RealEstatePipeline.Model;
using RealEstatePipeline.Services;

namespace RealEstatePipeline.Pages
{
    public class AgentDashboardModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ClientService _clientService;

        public bool IsAgent { get; private set; }
        public string UserId { get; private set; } // Property to store user ID

        public Agent_Info Agent { get; private set; }
        public List<ClientRegistration> SharedClients { get; set; }


        public AgentDashboardModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, ClientService clientService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _clientService = clientService; 
        }

        // Method to get client info, call this in your Razor view
        public async Task<ApplicationUser> GetClientInfoAsync(string clientId)
        {
            return await _clientService.GetClientByIdAsync(clientId);
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index"); // Redirect to the home page or login page
        }
        public async Task OnGetAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User) as Agent_Info;
                if (user != null)
                {
                    IsAgent = await _userManager.IsInRoleAsync(user, "Agent");
                    UserId = user.Id; // Store the user ID

                    // Retrieve shared clients for the agent
                    var sharedClients = await _context.SharedClients
                                                       .Where(r => r.AgentId == UserId)
                                                       .ToListAsync();
                    SharedClients = new List<ClientRegistration>(); // Initialize the list

                    foreach (var sharedClient in sharedClients)
                    {
                        // Optionally, fetch and store detailed client information here
                        var clientInfo = await GetClientInfoAsync(sharedClient.ClientId) as ClientRegistration;
                        if (clientInfo != null)
                        {
                            //Add the client to the list
                            SharedClients.Add(clientInfo);
                        }
                    }

                    Agent = user;
                }

            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error getting agent information: {ex.Message}");
            }

           
        }

        public async Task<IActionResult> OnGetProfilePictureAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId) as Agent_Info;
                if (user?.ProfilePicture != null)
                {
                    return File(user.ProfilePicture, "image/jpeg"); // Adjust the content type as needed
                }

                // Return a default image or NotFound as appropriate
                return File("~/images/default-profile.jpg", "image/jpeg"); // Example path to a default image

            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"Error getting profile picture: {ex.Message}");
            }

            
        }

       // public async Task<IActionResult> onSharedClient

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
