using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;
using System.ComponentModel.DataAnnotations;

namespace RealEstatePipeline.Pages
{
    public class EditClientModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<EditClientModel> _logger;
        public  EditClientModel(UserManager<ApplicationUser> userManager, ILogger<EditClientModel> logger) { 
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel ClientInput { get; set; }

        public ClientRegistration Client {  get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {

            // Get the currently authenticated user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !await _userManager.IsInRoleAsync(currentUser, "Client"))
            {
                return RedirectToPage("/Error");
            }

            Client = await _userManager.FindByIdAsync(id) as ClientRegistration;

            if (Client == null)
            {
                return RedirectToPage("/Success");
            }

            ClientInput = new InputModel
            {
                FirstName = Client.FirstName,
                LastName = Client.LastName,
                LocationPreference = Client.LocationPreference,
                ProfileDescription = Client.ProfileDescription,
                PreferredCommunicationMethod = Client.PreferredCommunicationMethod,
                // Parsing the delimited string for languages
                SpeaksEnglish = Client.PrimaryLanguage?.Split(',').Contains("English") ?? false,
                SpeaksSpanish = Client.PrimaryLanguage?.Split(',').Contains("Spanish") ?? false,
                SpeaksMandarin = Client.PrimaryLanguage?.Split(',').Contains("Mandarin") ?? false,
                SpeaksFrench = Client.PrimaryLanguage?.Split(',').Contains("French") ?? false,
                SpeaksArabic = Client.PrimaryLanguage?.Split(',').Contains("Arabic") ?? false,
                SpeaksRussian = Client.PrimaryLanguage?.Split(',').Contains("Russian") ?? false,
                SpeaksPortuguese = Client.PrimaryLanguage?.Split(',').Contains("Portuguese") ?? false,
                SpeaksGerman = Client.PrimaryLanguage?.Split(',').Contains("German") ?? false,
                SpeaksJapanese = Client.PrimaryLanguage?.Split(',').Contains("Japanese") ?? false,
                SpeaksHindi = Client.PrimaryLanguage?.Split(',').Contains("Hindi") ?? false,
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
                var clientToUpdate = await _userManager.FindByIdAsync(ClientInput.Id) as ClientRegistration;
                if (clientToUpdate == null)
                {
                    _logger.LogWarning($"Agent with ID {ClientInput.Id} not found.");
                    return NotFound();
                }

                // Update agent properties from ClientInput
                clientToUpdate.FirstName = ClientInput.FirstName;
                clientToUpdate.LastName = ClientInput.LastName;
                clientToUpdate.Email = ClientInput.Email;
                clientToUpdate.LocationPreference = ClientInput.LocationPreference;
                clientToUpdate.ProfileDescription = ClientInput.ProfileDescription;
                clientToUpdate.PreferredCommunicationMethod = ClientInput.PreferredCommunicationMethod;

                var selectedLanguages = new List<string>();
                if (ClientInput.SpeaksEnglish) selectedLanguages.Add("English");
                if (ClientInput.SpeaksSpanish) selectedLanguages.Add("Spanish");
                if (ClientInput.SpeaksMandarin) selectedLanguages.Add("Mandarin");
                if (ClientInput.SpeaksFrench) selectedLanguages.Add("French");
                if (ClientInput.SpeaksArabic) selectedLanguages.Add("Arabic");
                if (ClientInput.SpeaksRussian) selectedLanguages.Add("Russian");
                if (ClientInput.SpeaksPortuguese) selectedLanguages.Add("Portuguese");
                if (ClientInput.SpeaksGerman) selectedLanguages.Add("German");
                if (ClientInput.SpeaksJapanese) selectedLanguages.Add("Japanese");
                if (ClientInput.SpeaksHindi) selectedLanguages.Add("Hindi");
                // Add other languages as needed
                clientToUpdate.PrimaryLanguage = string.Join(",", selectedLanguages);



                var updateResult = await _userManager.UpdateAsync(clientToUpdate);
                if (updateResult.Succeeded)
                {
                    _logger.LogInformation($"Agent with ID {ClientInput.Id} updated successfully.");
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
                _logger.LogError(ex, $"An error occurred while updating the agent with ID {ClientInput.Id}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating your profile.");
            }

            return Page();
        }


    }

    public class InputModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string? LocationPreference { get; set; }

        public string? ProfileDescription { get; set; } // Optional

        public string? PreferredCommunicationMethod { get; set; }

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
        public bool IsResidential { get; set; }
        public bool IsCommercial { get; set; }
        public bool IsIndustrial { get; set; }
        public bool IsLand { get; set; }
        public bool IsSpecialPurpose { get; set; }
    }

}


