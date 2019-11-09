using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeploymentTrackerCore.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HostName = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deployments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeployedEnvironmentId = table.Column<int>(nullable: true),
                    BranchName = table.Column<string>(maxLength: 200, nullable: false),
                    PublicURL = table.Column<string>(maxLength: 150, nullable: false),
                    Status = table.Column<string>(nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    CreatedBy_Name = table.Column<string>(maxLength: 150, nullable: false),
                    CreatedBy_UserName = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedBy_Timestamp = table.Column<DateTime>(nullable: false),
                    ModifiedBy_Name = table.Column<string>(maxLength: 150, nullable: false),
                    ModifiedBy_UserName = table.Column<string>(maxLength: 50, nullable: false),
                    ModifiedBy_Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deployments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deployments_Environments_DeployedEnvironmentId",
                        column: x => x.DeployedEnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_DeployedEnvironmentId",
                table: "Deployments",
                column: "DeployedEnvironmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deployments");

            migrationBuilder.DropTable(
                name: "Environments");
        }
    }
}
