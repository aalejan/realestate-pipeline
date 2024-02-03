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



    }
}
