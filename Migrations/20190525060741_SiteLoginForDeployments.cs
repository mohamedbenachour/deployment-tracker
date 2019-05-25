using Microsoft.EntityFrameworkCore.Migrations;

namespace deployment_tracker.Migrations
{
    public partial class SiteLoginForDeployments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteLogin_Password",
                table: "Deployments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteLogin_UserName",
                table: "Deployments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteLogin_Password",
                table: "Deployments");

            migrationBuilder.DropColumn(
                name: "SiteLogin_UserName",
                table: "Deployments");
        }
    }
}
