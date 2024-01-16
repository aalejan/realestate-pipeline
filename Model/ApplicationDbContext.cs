using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RealEstatePipeline.Model
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        // Constructor for the ApplicationDbContext.
        // DbContextOptions<ApplicationDbContext> options: The options to be used by the DbContext. These options usually include the database provider (like SQL Server), connection string, and other settings.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Calls the base class constructor of DbContext, passing in the options. This ensures that our ApplicationDbContext inherits all the functionality of a DbContext, configured with the specified options.
        {
            // The body of the constructor is empty because all necessary configuration is done through the options parameter and handled by the base class.
        }

        // DbSet properties for your entities here
        public DbSet<Client_Info> RealEstateInquiries { get; set; }
        public DbSet<AgentRating> AgentRatings { get; set; }

     


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customize the model here

            // For example, you can remove the 'AspNetRoles' table from the model if it's not needed
            

            modelBuilder.Entity<ApplicationUser>()
       .HasDiscriminator<string>("UserType")
       .HasValue<ApplicationUser>("User")
       .HasValue<Agent_Info>("Agent")
       .HasValue<ClientRegistration>("Client");

            // Define the one-to-many relationship between Agent and Ratings
            modelBuilder.Entity<Agent_Info>()
                .HasMany(a => a.AgentRatings)
                .WithOne(r => r.Agent)
                .HasForeignKey(r => r.AgentId);


        }

    }
}
