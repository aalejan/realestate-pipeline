using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RealEstatePipeline.Model
{
    public class Agent_Info : ApplicationUser
    {

        public Agent_Info()
        {
            AgentRatings = new HashSet<AgentRating>();
        }
        public int? YearsOfExperience { get; set; }
        public double? Ratings { get; set; } // Average rating

        // Navigation property for ratings
        public virtual ICollection<AgentRating> AgentRatings { get; set; }

        public byte[] ProfilePicture { get; set; }

    }
}
