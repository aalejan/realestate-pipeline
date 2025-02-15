using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;
using System.ComponentModel.DataAnnotations;

namespace RealEstatePipeline.Pages
{
    public class EditAgentModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EditAgentModel> _logger;
        public EditAgentModel(UserManager<ApplicationUser> userManager, ILogger<EditAgentModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel AgentEdit { get; set; } // For POST
        public Agent_Info Agent { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Get the currently authenticated user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !await _userManager.IsInRoleAsync(currentUser, "Agent"))
            {
                return RedirectToPage("/Error");
            }

            // Load the agent data
            Agent = await _userManager.FindByIdAsync(id) as Agent_Info;

            if (Agent == null)
            {
                return RedirectToPage("/Error");
            }

            // Additional checks if necessary
            if (Agent.Id != currentUser.Id)
            {
                return RedirectToPage("/Error");
            }

            // Populate AgentEdit with the current values
            AgentEdit = new InputModel
            {
                Id = Agent.Id,
                FirstName = Agent.FirstName,
                LastName = Agent.LastName,
                Email = Agent.Email,
                YearsOfExperience = (int)Agent.YearsOfExperience,
                LocationPreference = Agent.LocationPreference,
                ProfileDescription = Agent.ProfileDescription,
                PreferredCommunicationMethod = Agent.PreferredCommunicationMethod,
                // Parsing the delimited string for languages
                SpeaksEnglish = Agent.PrimaryLanguage?.Split(',').Contains("English") ?? false,
                SpeaksSpanish = Agent.PrimaryLanguage?.Split(',').Contains("Spanish") ?? false,
                SpeaksMandarin = Agent.PrimaryLanguage?.Split(',').Contains("Mandarin") ?? false,
                SpeaksFrench = Agent.PrimaryLanguage?.Split(',').Contains("French") ?? false,
                SpeaksArabic = Agent.PrimaryLanguage?.Split(',').Contains("Arabic") ?? false,
                SpeaksRussian = Agent.PrimaryLanguage?.Split(',').Contains("Russian") ?? false,
                SpeaksPortuguese = Agent.PrimaryLanguage?.Split(',').Contains("Portuguese") ?? false,
                SpeaksGerman = Agent.PrimaryLanguage?.Split(',').Contains("German") ?? false,
                SpeaksJapanese = Agent.PrimaryLanguage?.Split(',').Contains("Japanese") ?? false,
                SpeaksHindi = Agent.PrimaryLanguage?.Split(',').Contains("Hindi") ?? false,
                // Add more languages as needed

            };


            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Starting to update agent.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid.");
                return Page();
            }

            try
            {
                var agentToUpdate = await _userManager.FindByIdAsync(AgentEdit.Id) as Agent_Info;
                if (agentToUpdate == null)
                {
                    _logger.LogWarning($"Agent with ID {AgentEdit.Id} not found.");
                    return NotFound();
                }

                // Update agent properties from AgentEdit
                agentToUpdate.FirstName = AgentEdit.FirstName;
                agentToUpdate.LastName = AgentEdit.LastName;
                agentToUpdate.Email = AgentEdit.Email;
                agentToUpdate.YearsOfExperience = AgentEdit.YearsOfExperience;
                agentToUpdate.LocationPreference = AgentEdit.LocationPreference;
                agentToUpdate.ProfileDescription = AgentEdit.ProfileDescription;
                agentToUpdate.PreferredCommunicationMethod = AgentEdit.PreferredCommunicationMethod;


                ParseLanguages(AgentEdit, agentToUpdate);

                var updateResult = await _userManager.UpdateAsync(agentToUpdate);
                if (updateResult.Succeeded)
                {
                    _logger.LogInformation($"Agent with ID {AgentEdit.Id} updated successfully.");
                    return RedirectToPage("/AgentDashboard");
                }

                foreach (var error in updateResult.Errors)
                {
                    _logger.LogError($"Error updating agent: {error.Description}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the agent with ID {AgentEdit.Id}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating your profile.");
            }

            return Page();
        }

        private void ParseLanguages(InputModel model, Agent_Info agent)
        {
            string[] languages = new[] { "English", "Spanish", "Mandarin", "French", "Arabic", "Russian", "Portuguese", "German", "Japanese", "Hindi" };
            var selectedLanguages = new List<string>();


            foreach (var language in languages)
            {
                //GetType finds out the model type (InputModel) and then allows you to get property bc it knows it's InputModel
                var property = model.GetType().GetProperty($"Speaks{language}");
                if (property != null && (bool)property.GetValue(model))
                {
                    selectedLanguages.Add(language);
                }
            }

            agent.PrimaryLanguage = string.Join(",", selectedLanguages);


        }

        public class InputModel
        {
            public string Id { get; set; }

            [Required, EmailAddress]
            public required string Email { get; set; }

            [Required]
            public required string FirstName { get; set; }

            [Required]
            public required string LastName { get; set; }

            [Range(0, 100)]
            public int YearsOfExperience { get; set; }

            public string? LocationPreference { get; set; }
            public string? ProfileDescription { get; set; }
            public string? PreferredCommunicationMethod { get; set; }

            // Language properties
            public bool SpeaksEnglish { get; set; }
            public bool SpeaksSpanish { get; set; }
            public bool SpeaksMandarin { get; set; }
            public bool SpeaksFrench { get; set; }
            public bool SpeaksArabic { get; set; }
            public bool SpeaksRussian { get; set; }
            public bool SpeaksPortuguese { get; set; }
            public bool SpeaksGerman { get; set; }
            public bool SpeaksJapanese { get; set; }
            public bool SpeaksHindi { get; set; }

            // Property type preferences
            public bool IsResidential { get; set; }
            public bool IsCommercial { get; set; }
            public bool IsIndustrial { get; set; }
            public bool IsLand { get; set; }
            public bool IsSpecialPurpose { get; set; }

        }

    }

}


