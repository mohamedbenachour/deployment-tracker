using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeploymentTrackerCore.Migrations
{
    public partial class DeploymentNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeploymentNote",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    DeploymentId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedBy_Name = table.Column<string>(maxLength: 150, nullable: true),
                    CreatedBy_UserName = table.Column<string>(maxLength: 50, nullable: true),
                    CreatedBy_Timestamp = table.Column<DateTime>(nullable: true),
                    ModifiedBy_Name = table.Column<string>(maxLength: 150, nullable: true),
                    ModifiedBy_UserName = table.Column<string>(maxLength: 50, nullable: true),
                    ModifiedBy_Timestamp = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeploymentNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeploymentNote_Deployments_DeploymentId",
                        column: x => x.DeploymentId,
                        principalTable: "Deployments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeploymentNote_DeploymentId",
                table: "DeploymentNote",
                column: "DeploymentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeploymentNote");
        }
    }
}
