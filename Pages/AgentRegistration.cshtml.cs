using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RealEstatePipeline.Model;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace RealEstatePipeline.Pages
{
    public class AgentRegistrationModel : PageModel
    {

        
        private readonly UserManager<ApplicationUser> _userManager; // Use UserManager for Identity
        private readonly ILogger<AgentRegistrationModel> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AgentRegistrationModel(UserManager<ApplicationUser> userManager, ILogger<AgentRegistrationModel> logger, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;
            
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string PropertyTypes { get; set; }
        public string LanguagesSpoken { get; set; }
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

            [Required]
            public int YearsOfExperience { get; set; }

            public string? LocationPreference { get; set; }

            public string? ProfileDescription { get; set; } // Optional

            public string? PreferredCommunicationMethod { get; set; }

            public IFormFile? ProfilePicture { get; set; }


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

                byte[] profilePictureData = null;
                if (Input.ProfilePicture != null && Input.ProfilePicture.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Input.ProfilePicture.CopyToAsync(memoryStream);
                        profilePictureData = memoryStream.ToArray();
                    }
                }


                var agent = new Agent_Info
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    ProfileDescription = Input.ProfileDescription,
                    LocationPreference = Input.LocationPreference,
                    YearsOfExperience = Input.YearsOfExperience,
                    PropertyTypes = string.Join(",", propertyTypes),
                    PreferredCommunicationMethod = Input.PreferredCommunicationMethod,
                    PrimaryLanguage = languagesSpoken,
                    ProfilePicture = profilePictureData
                };

                var result = await _userManager.CreateAsync(agent, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Check if the 'Agent' role exists
                    var roleExists = await _roleManager.RoleExistsAsync("Agent");
                    if (!roleExists)
                    {
                        // Create the 'Agent' role if it doesn't exist
                        await _roleManager.CreateAsync(new IdentityRole("Agent"));
                    }

                    // Assign the 'Agent' role to the newly created agent
                    await _userManager.AddToRoleAsync(agent, "Agent");

                    await _signInManager.SignInAsync(agent, isPersistent: true);
                    // Post-registration logic
                    return RedirectToPage("AgentDashboard");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("Input.Email", "This email is already taken.");
                        }else if (error.Code.Contains("Password"))
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
