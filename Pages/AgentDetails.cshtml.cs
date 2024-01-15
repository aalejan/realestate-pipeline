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

       //Write a method that allows client to give an agent a rating. Only client can do this. Users are stored using identity user.




        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user != null && await _userManager.IsInRoleAsync(user, "Agent"))
            {
                Agent = user as Agent_Info;
            }

            if (Agent == null)
            {
                return NotFound();
            }


            return Page();
        }
    }

}
