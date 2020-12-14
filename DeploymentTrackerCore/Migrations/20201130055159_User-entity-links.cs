using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeploymentTrackerCore.Migrations
{
    public partial class Userentitylinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEntityLinks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TargetUserName = table.Column<string>(maxLength: 50, nullable: false),
                    ReferencedEntity = table.Column<string>(maxLength: 200, nullable: false),
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
                    table.PrimaryKey("PK_UserEntityLinks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEntityLinks_IsActive",
                table: "UserEntityLinks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntityLinks_TargetUserName",
                table: "UserEntityLinks",
                column: "TargetUserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEntityLinks");
        }
    }
}
