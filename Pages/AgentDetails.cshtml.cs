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

        public AgentDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Agent_Info Agent { get; set; }

        [BindProperty]
        public string AgentId { get; set; } // This will be bound to the hidden input in the form


        [BindProperty]
        public int NewRating { get; set; } // For binding the new rating value from the form

        [BindProperty]
        public string NewComment { get; set; } // For binding the new comment from the form


        //Write a method that allows client to give an agent a rating. Only client can do this. Users are stored using identity user.



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
            }

            if (Agent == null)
            {
                return NotFound();
            }


            return Page();
        }
    }

}
