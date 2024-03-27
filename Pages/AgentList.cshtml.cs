using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RealEstatePipeline.Model;

namespace RealEstatePipeline.Pages
{
    public class AgentListModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // UserManager to check roles
        private readonly ILogger<AgentListModel> _logger;

        public AgentListModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AgentListModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public IList<Agent_Info> Agents { get; private set; } = new List<Agent_Info>();
        public List<AgentMatchViewModel> MatchedAgents { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null && await _userManager.IsInRoleAsync(user, "Client"))
                {
                    var client = user as ClientRegistration;
                    MatchedAgents = await GetMatchedAgentsAsync(client);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to retrieve matched agents for the user.");

            }


            return Page();
        }
        public async Task OnGetAsync(string searchQuery = null)
        {
            IQueryable<Agent_Info> query = _context.Users.OfType<Agent_Info>();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(a => a.UserName.Contains(searchQuery) ||
                                         a.Email.Contains(searchQuery) ||
                                         (a.FirstName != null && a.FirstName.Contains(searchQuery)) ||
                                         (a.LastName != null && a.LastName.Contains(searchQuery)));
            }

            Agents = await query.ToListAsync();
        }

        private async Task<List<AgentMatchViewModel>> GetMatchedAgentsAsync(ClientRegistration client)
        {
            // Implement your matching logic here
            var agents = await _context.Users.OfType<Agent_Info>().ToListAsync();
            var matchedAgents = agents.Select(agent => new AgentMatchViewModel
            {
                Agent = agent,
                MatchScore = CalculateMatchScore(agent, client)
            })
            .OrderByDescending(x => x.MatchScore)
            .ToList();

            return matchedAgents;
        }

        private int CalculateMatchScore(Agent_Info agent, ClientRegistration client)
        {
            int score = 0;

            if (client.LocationPreference == agent.LocationPreference)
                score++;

            if (client.PreferredCommunicationMethod == agent.PreferredCommunicationMethod)
                score++;

            var clientPropertyTypes = client.PropertyTypes?.Split(',') ?? new string[0];
            var agentPropertyTypes = agent.PropertyTypes?.Split(',') ?? new string[0];
            if (clientPropertyTypes.Intersect(agentPropertyTypes).Any())
                score++;

            var clientLanguages = client.PrimaryLanguage?.Split(',') ?? new string[0];
            var agentLanguages = agent.PrimaryLanguage?.Split(',') ?? new string[0];
            if (clientLanguages.Intersect(agentLanguages).Any())
                score++;

            return score;
        }



    }
}


public class AgentMatchViewModel
{
    public Agent_Info Agent { get; set; }
    public int MatchScore { get; set; }
}