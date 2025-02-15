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
        private readonly ILogger<AgentDashboardModel> _logger;

        public bool IsAgent { get; private set; }
        public string UserId { get; private set; }
        public Agent_Info Agent { get; private set; }
        public List<ClientRegistration> SharedClients { get; private set; } = new();

        public AgentDashboardModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            ClientService clientService,
            ILogger<AgentDashboardModel> logger)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ApplicationUser> GetClientInfoAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentException("Client ID cannot be null or empty", nameof(clientId));
            }

            return await _clientService.GetClientByIdAsync(clientId);
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user is not Agent_Info agentInfo)
                {
                    _logger.LogWarning("User {UserId} is not an Agent_Info", user?.Id);
                    return RedirectToPage("/AccessDenied");
                }

                IsAgent = await _userManager.IsInRoleAsync(user, "Agent");
                if (!IsAgent)
                {
                    _logger.LogWarning("User {UserId} is not in Agent role", user.Id);
                    return RedirectToPage("/AccessDenied");
                }

                UserId = agentInfo.Id;
                Agent = agentInfo;

                await LoadSharedClientsAsync();

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in OnGetAsync for user {UserId}", UserId);
                throw;
            }
        }

        private async Task LoadSharedClientsAsync()
        {
            var sharedClients = await _context.SharedClients
                .Where(r => r.AgentId == UserId)
                .AsNoTracking()
                .ToListAsync();

            var clientTasks = sharedClients.Select(async sc =>
            {
                var clientInfo = await GetClientInfoAsync(sc.ClientId) as ClientRegistration;
                return clientInfo;
            });

            var clients = await Task.WhenAll(clientTasks);
            SharedClients = clients.Where(c => c != null).ToList();
        }

        public async Task<IActionResult> OnGetProfilePictureAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId) as Agent_Info;
                if (user?.ProfilePicture != null)
                {
                    return File(user.ProfilePicture, "image/jpeg");
                }

                return File("~/images/default-profile.jpg", "image/jpeg");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile picture for user {UserId}", userId);
                return StatusCode(500, "Error retrieving profile picture");
            }
        }

        public bool IsUserLoggedIn() => User?.Identity?.IsAuthenticated ?? false;

        public async Task<UserProfileInfo> GetUserProfileAsync()
        {
            if (!IsUserLoggedIn())
            {
                return null;
            }

            var user = await _userManager.GetUserAsync(User);
            return new UserProfileInfo
            {
                Username = user?.UserName,
                Email = user?.Email,
                FirstName = user?.FirstName
            };
        }
    }

    public class UserProfileInfo
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
    }
}