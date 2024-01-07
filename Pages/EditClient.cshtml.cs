using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealEstatePipeline.Model;
using System.ComponentModel.DataAnnotations;

namespace RealEstatePipeline.Pages
{
    public class EditClientModel : PageModel
    {

        [BindProperty]
        public InputModel Input { get; set; }

        public ClientRegistration Client {  get; set; }

        public void OnGet()
        {
        }
    }

    public class InputModel
    {
       
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


