using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;
using System.ComponentModel.DataAnnotations;

namespace RealEstatePipeline.Pages
{
    public class ClientRegistrationModel : PageModel
    {


        private readonly UserManager<ApplicationUser> _userManager; // Use UserManager for Identity
        private readonly ILogger<AgentRegistrationModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ClientRegistrationModel(UserManager<ApplicationUser> userManager, ILogger<AgentRegistrationModel> logger, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;

        }

        [BindProperty]
        public InputModel Input { get; set; }

       public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            public string? LocationPreference { get; set; }

            public string? ProfileDescription { get; set; } // Optional

            public string? PreferredCommunicationMethod { get; set; }

            public decimal? MinimumBudget { get; set; }
            public decimal? MaximumBudget { get; set; }

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

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var propertyTypes = new List<string>();
                if (Input.IsResidential) propertyTypes.Add("Residential");
                if (Input.IsCommercial) propertyTypes.Add("Commercial");
                if (Input.IsIndustrial) propertyTypes.Add("Industrial");
                if (Input.IsLand) propertyTypes.Add("Land");
                if (Input.IsSpecialPurpose) propertyTypes.Add("Special Purpose");

                var selectedLanguages = new List<string>();
                if (Input.SpeaksEnglish) selectedLanguages.Add("English");
                if (Input.SpeaksSpanish) selectedLanguages.Add("Spanish");
                if (Input.SpeaksMandarin) selectedLanguages.Add("Mandarin");
                if (Input.SpeaksFrench) selectedLanguages.Add("French");
                if (Input.SpeaksArabic) selectedLanguages.Add("Arabic");
                if (Input.SpeaksRussian) selectedLanguages.Add("Russian");
                if (Input.SpeaksPortuguese) selectedLanguages.Add("Portuguese");
                if (Input.SpeaksGerman) selectedLanguages.Add("German");
                if (Input.SpeaksJapanese) selectedLanguages.Add("Japanese");
                if (Input.SpeaksHindi) selectedLanguages.Add("Hindi");


                var languagesSpoken = string.Join(",", selectedLanguages);

                var agent = new ClientRegistration
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    ProfileDescription = Input.ProfileDescription,
                    LocationPreference = Input.LocationPreference,
                    PropertyTypes = string.Join(",", propertyTypes),
                    PreferredCommunicationMethod = Input.PreferredCommunicationMethod,
                    MinimumBudget = Input.MinimumBudget,
                    MaximumBudget = Input.MaximumBudget,
                    PrimaryLanguage = languagesSpoken
                };

                var result = await _userManager.CreateAsync(agent, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Check if the 'Agent' role exists
                    var roleExists = await _roleManager.RoleExistsAsync("Client");
                    if (!roleExists)
                    {
                        // Create the 'Agent' role if it doesn't exist
                        await _roleManager.CreateAsync(new IdentityRole("Client"));
                    }

                    // Assign the 'Agent' role to the newly created agent
                    await _userManager.AddToRoleAsync(agent, "Client");

                    await _signInManager.SignInAsync(agent, isPersistent: true);
                    // Post-registration logic
                    return RedirectToPage("AgentList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("Input.Email", "This email is already taken.");
                        }
                        else if (error.Code.Contains("Password"))
                        {
                            ModelState.AddModelError("Input.Password", error.Description);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

            }

            // Log general model state errors
            if (!ModelState.IsValid)
            {
                _logger.LogError("Model state is invalid. Errors:");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError($"- {state.Key}: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            return Page(); // Stay on the same page and show errors
        }



    }
}
