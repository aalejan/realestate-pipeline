using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RealEstatePipeline.Model
{
    public class ClientRegistration : ApplicationUser
    {
       

        [PersonalData]
        public string Address { get; set; }

        
        // Separating the budget into minimum and maximum
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MinimumBudget { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? MaximumBudget { get; set; }
    }
}
