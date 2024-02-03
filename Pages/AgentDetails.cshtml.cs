using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RealEstatePipeline.Model;

namespace RealEstatePipeline.Pages
{
    // AgentDetails.cshtml.cs
    public class AgentDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // UserManager to check roles
        private readonly ILogger<AgentDetailsModel> _logger;

        public AgentDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AgentDetailsModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public Agent_Info Agent { get; set; }

        public List<AgentRating> AgentRatings { get; set; }

        [BindProperty]
        public string AgentId { get; set; } // This will be bound to the hidden input in the form


        [BindProperty]
        public int NewRating { get; set; } // For binding the new rating value from the form

        [BindProperty]
        public string NewComment { get; set; } // For binding the new comment from the form


        public async Task<IActionResult> OnPostShareClientProfileAsync()
        {
            _logger.LogInformation("Attempting to share client profile");

            var clientUser = await _userManager.GetUserAsync(User);
            if (clientUser == null)
            {
                _logger.LogWarning("Failed to find the client user.");
                return Unauthorized();
            }

            if (!await _userManager.IsInRoleAsync(clientUser, "Client"))
            {
                _logger.LogWarning("User {UserId} is not in the 'Client' role.", clientUser.Id);
                return Unauthorized();
            }

            var newSharedClient = new SharedClient
            {
                ClientId = clientUser.Id,
                AgentId = AgentId,
                SharedDate = DateTimeOffset.UtcNow
            };

            _logger.LogInformation("Sharing client profile with agent. ClientId: {ClientId}, AgentId: {AgentId}", clientUser.Id, AgentId);

            _context.SharedClients.Add(newSharedClient);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully shared client profile with agent.");

            return RedirectToPage(); // Or redirect to a confirmation/thank you page
        }


        public async Task<IActionResult> OnPostRateAgentAsync()
        {
            var clientUser = await _userManager.GetUserAsync(User);
            if (clientUser == null || !await _userManager.IsInRoleAsync(clientUser, "Client"))
            {
                return Unauthorized(); // Only clients can rate
            }

            if (NewRating < 1 || NewRating > 5)
            {
                return BadRequest("Invalid rating value.");
            }
            

            var newRating = new AgentRating
            {
                ClientId = clientUser.Id,
                AgentId = AgentId,
                Rating = NewRating,
                Comments = NewComment,
                RatingDate = DateTimeOffset.UtcNow
            };
            _context.AgentRatings.Add(newRating);
            await _context.SaveChangesAsync();

            // Optionally, recalculate and update the average rating for the agent
            // ...

            return RedirectToPage(); // Or redirect to a confirmation/thank you page
        }
        public async Task<IActionResult> OnGetAsync(string id)
        {


            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user != null && await _userManager.IsInRoleAsync(user, "Agent"))
            {
                Agent = user as Agent_Info;
                AgentId = Agent.Id; // Set the AgentId for the form

                // Retrieve ratings for the agent
                AgentRatings = await _context.AgentRatings
                                            .Where(r => r.AgentId == id)
                                            .ToListAsync();
            }



            if (Agent == null)
            {
                return NotFound();
            }


            return Page();
        }
    }

}
