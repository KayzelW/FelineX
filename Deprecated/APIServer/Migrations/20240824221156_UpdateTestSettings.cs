using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTestSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestSettingsUser",
                columns: table => new
                {
                    TestSettingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestUsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSettingsUser", x => new { x.TestSettingsId, x.TestUsersId });
                    table.ForeignKey(
                        name: "FK_TestSettingsUser_TestSettings_TestSettingsId",
                        column: x => x.TestSettingsId,
                        principalTable: "TestSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestSettingsUser_Users_TestUsersId",
                        column: x => x.TestUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestSettingsUser_TestUsersId",
                table: "TestSettingsUser",
                column: "TestUsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestSettingsUser");
        }
    }
}
