using Microsoft.EntityFrameworkCore.Migrations;

namespace deployment_tracker.Migrations
{
    public partial class StoreDeploymentPublicURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicURL",
                table: "Deployments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicURL",
                table: "Deployments");
        }
    }
}
