using Microsoft.EntityFrameworkCore.Migrations;

namespace DeploymentTrackerCore.Migrations
{
    public partial class TypeTeardownTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeardownTemplate",
                table: "Types",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeardownTemplate",
                table: "Types");
        }
    }
}
