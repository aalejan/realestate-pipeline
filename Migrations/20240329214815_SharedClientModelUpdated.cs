using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstatePipeline.Migrations
{
    /// <inheritdoc />
    public partial class SharedClientModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasFoundHouse",
                table: "SharedClients",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasSignedContract",
                table: "SharedClients",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsContacted",
                table: "SharedClients",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "SharedClients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFoundHouse",
                table: "SharedClients");

            migrationBuilder.DropColumn(
                name: "HasSignedContract",
                table: "SharedClients");

            migrationBuilder.DropColumn(
                name: "IsContacted",
                table: "SharedClients");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "SharedClients");
        }
    }
}
