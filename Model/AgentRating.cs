namespace RealEstatePipeline.Model
{
    public class AgentRating
    {
        public int Id { get; set; }
        public string ClientId { get; set; } // ForeignKey for the client

        // Foreign key for Agent
        public string AgentId { get; set; }

        // Navigation property for Agent
        public virtual Agent_Info Agent { get; set; }

        public int Rating { get; set; }
        public string Comments { get; set; }

        // Date-time stamp for when the rating was given
        public DateTimeOffset RatingDate { get; set; }
    }

}
