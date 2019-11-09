using Microsoft.EntityFrameworkCore.Migrations;

namespace DeploymentTrackerCore.Migrations
{
    public partial class AddSiteNameToDeployments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                table: "Deployments",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "Deployments");
        }
    }
}
