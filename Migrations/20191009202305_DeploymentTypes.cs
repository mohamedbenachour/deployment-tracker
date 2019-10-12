using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace deployment_tracker.Migrations
{
    public partial class DeploymentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Deployments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Default" }
                );

            migrationBuilder.UpdateData(
                table: "Deployments",
                keyColumn: "TypeId",
                keyValue: null,
                column: "TypeId",
                value: 1
            );

            migrationBuilder.CreateIndex(
                name: "IX_Deployments_TypeId",
                table: "Deployments",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropIndex(
                name: "IX_Deployments_TypeId",
                table: "Deployments");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Deployments");
        }
    }
}
