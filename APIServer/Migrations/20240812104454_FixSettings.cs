using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class FixSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_TestSettings_TestSettingsId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_ThemeTasks_TestSettings_TestSettingsId",
                table: "ThemeTasks");

            migrationBuilder.DropIndex(
                name: "IX_ThemeTasks_TestSettingsId",
                table: "ThemeTasks");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TestSettingsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TestSettingsId",
                table: "ThemeTasks");

            migrationBuilder.DropColumn(
                name: "TestSettingsId",
                table: "Groups");

            migrationBuilder.CreateTable(
                name: "TestSettingsThemeTask",
                columns: table => new
                {
                    TasksThemesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestSettingsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSettingsThemeTask", x => new { x.TasksThemesId, x.TestSettingsId });
                    table.ForeignKey(
                        name: "FK_TestSettingsThemeTask_TestSettings_TestSettingsId",
                        column: x => x.TestSettingsId,
                        principalTable: "TestSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestSettingsThemeTask_ThemeTasks_TasksThemesId",
                        column: x => x.TasksThemesId,
                        principalTable: "ThemeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestSettingsUserGroup",
                columns: table => new
                {
                    TestGroupsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestSettingsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSettingsUserGroup", x => new { x.TestGroupsId, x.TestSettingsId });
                    table.ForeignKey(
                        name: "FK_TestSettingsUserGroup_Groups_TestGroupsId",
                        column: x => x.TestGroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestSettingsUserGroup_TestSettings_TestSettingsId",
                        column: x => x.TestSettingsId,
                        principalTable: "TestSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestSettingsThemeTask_TestSettingsId",
                table: "TestSettingsThemeTask",
                column: "TestSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSettingsUserGroup_TestSettingsId",
                table: "TestSettingsUserGroup",
                column: "TestSettingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestSettingsThemeTask");

            migrationBuilder.DropTable(
                name: "TestSettingsUserGroup");

            migrationBuilder.AddColumn<Guid>(
                name: "TestSettingsId",
                table: "ThemeTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TestSettingsId",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThemeTasks_TestSettingsId",
                table: "ThemeTasks",
                column: "TestSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TestSettingsId",
                table: "Groups",
                column: "TestSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_TestSettings_TestSettingsId",
                table: "Groups",
                column: "TestSettingsId",
                principalTable: "TestSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThemeTasks_TestSettings_TestSettingsId",
                table: "ThemeTasks",
                column: "TestSettingsId",
                principalTable: "TestSettings",
                principalColumn: "Id");
        }
    }
}
