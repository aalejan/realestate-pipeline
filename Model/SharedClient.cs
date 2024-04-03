namespace RealEstatePipeline.Model
{
    public class SharedClient
    {
        public int Id { get; set; }

        public string ClientId { get; set; } // ForeignKey for the client
     
   

        // Foreign key for Agent
        public string AgentId { get; set; }

        // Navigation property for Agent
        public virtual Agent_Info Agent { get; set; }

        // Date-time stamp for when the rating was given
        public DateTimeOffset SharedDate { get; set; }

        // Indicates whether the agent has contacted the client
        public bool IsContacted { get; set; }

        // Indicates whether the client has signed a contract
        public bool HasSignedContract { get; set; }

        // Indicates whether a house has been found for the client
        public bool HasFoundHouse { get; set; }

        // Optionally, you can add a field to add notes by the agent about the client
        public string Notes { get; set; }

    }
}
