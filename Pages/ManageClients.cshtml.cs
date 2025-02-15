using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RealEstatePipeline.Model;
using RealEstatePipeline.Services;
using RealEstatePipeline.ViewModels;

namespace RealEstatePipeline.Pages
{
    public class ManageClientsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ClientService _clientService;
        private readonly ILogger<ManageClientsModel> _logger;


        public List<SharedClientViewModel> SharedClients { get; set; }
        [BindProperty]
        public List<ClientUpdateViewModel> ClientUpdates { get; set; }

        public ManageClientsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ClientService clientService, ILogger<ManageClientsModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _clientService = clientService;
            _logger = logger;
            SharedClients = new List<SharedClientViewModel>();
            ClientUpdates = new List<ClientUpdateViewModel>();
        }

      


        public async Task OnGetAsync()
        {

           

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogWarning("User not found.");
                return;
            }

            _logger.LogInformation($"Loading shared clients for user {user.Id}");

            var sharedClients = await _context.SharedClients
                                              .Where(sc => sc.AgentId == user.Id)
                                              .ToListAsync();


            foreach (var sharedClient in sharedClients)
            {
                _logger.LogInformation($"Processing shared client {sharedClient.Id} for user {user.Id}.");
                var clientInfo = await GetClientInfoAsync(sharedClient.ClientId) as ClientRegistration;
                if (clientInfo != null)
                {
                    SharedClients.Add(new SharedClientViewModel
                    {
                        SharedClient = sharedClient,
                        ClientRegistration = clientInfo
                    });
                    _logger.LogInformation($"Added shared client {sharedClient.Id} with client info {clientInfo.Id}.");

                    // Initialize the ClientUpdates with the current state of the shared clients
                    ClientUpdates.Add(new ClientUpdateViewModel
                    {
                        Id = sharedClient.Id,
                        HasFoundHouse = sharedClient.HasFoundHouse,
                        IsContacted = sharedClient.IsContacted,
                        HasSignedContract = sharedClient.HasSignedContract,
                        Notes = sharedClient.Notes // Populate the notes
                    });

                }
                else
                {
                    _logger.LogWarning($"Client info for shared client {sharedClient.Id} is null.");
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                // Log model state errors
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogError(error.ErrorMessage);
                    }
                }
            }

            foreach (var update in ClientUpdates)
            {
                var sharedClient = await _context.SharedClients.FindAsync(update.Id);
                if (sharedClient != null)
                {
                    _logger.LogInformation($"Updating shared client {sharedClient.Id}");
                    sharedClient.HasFoundHouse = update.HasFoundHouse;
                    sharedClient.IsContacted = update.IsContacted;
                    sharedClient.HasSignedContract = update.HasSignedContract;
                    sharedClient.Notes = update.Notes;

                    // Update other properties as necessary
                }
                else
                {
                    _logger.LogWarning($"Shared client {update.Id} not found.");
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Changes saved to the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving changes: {ex.Message}");
            }

            return RedirectToPage();
        }





        // Method to get client info, call this in your Razor view
        public async Task<ApplicationUser> GetClientInfoAsync(string clientId)
        {
            return await _clientService.GetClientByIdAsync(clientId);
        }

        //Get shared client specific data
        public async Task<SharedClient> GetSharedClientByIdAsync(int sharedClientId)
        {
            return await _context.SharedClients.FindAsync(sharedClientId);
        }

        public class ClientUpdateViewModel
        {
            public int Id { get; set; }
            // Indicates whether the agent has contacted the client
            public bool IsContacted { get; set; }

            // Indicates whether the client has signed a contract
            public bool HasSignedContract { get; set; }

            // Indicates whether a house has been found for the client
            public bool HasFoundHouse { get; set; }
            public string Notes { get;  set; }
        }


    }
}
