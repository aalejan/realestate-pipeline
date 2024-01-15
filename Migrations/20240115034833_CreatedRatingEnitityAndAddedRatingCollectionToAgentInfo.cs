using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstatePipeline.Migrations
{
    /// <inheritdoc />
    public partial class CreatedRatingEnitityAndAddedRatingCollectionToAgentInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "AspNetUsers",
                newName: "Ratings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ratings",
                table: "AspNetUsers",
                newName: "Rating");
        }
    }
}
