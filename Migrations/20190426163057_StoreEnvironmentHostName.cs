using Microsoft.EntityFrameworkCore.Migrations;

namespace deployment_tracker.Migrations
{
    public partial class StoreEnvironmentHostName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HostName",
                table: "Environments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HostName",
                table: "Environments");
        }
    }
}
