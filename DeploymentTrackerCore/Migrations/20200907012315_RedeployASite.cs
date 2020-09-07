using Microsoft.EntityFrameworkCore.Migrations;

namespace DeploymentTrackerCore.Migrations
{
    public partial class RedeployASite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeploymentTemplate",
                table: "Types",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeploymentTemplate",
                table: "Types");
        }
    }
}
