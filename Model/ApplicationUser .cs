using Microsoft.AspNetCore.Identity;

namespace RealEstatePipeline.Model
{

    //Shared properties for all users
    //base class for all users and inherits from IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? LocationPreference { get; set; }
        public string? ProfileDescription { get; set; } // Bio or short description
        public string? PreferredCommunicationMethod { get; set; }
        public string? PropertyTypes { get; set; } // Delimited string, e.g., "Residential,Commercial"
        public string? PrimaryLanguage { get; set; }
    }

}
