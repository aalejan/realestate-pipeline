using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RealEstatePipeline.Model
{
    public class Agent_Info : ApplicationUser
    {
       

        public int? YearsOfExperience { get; set; }
        public double? Ratings { get; set; } // Average rating

    }
}
