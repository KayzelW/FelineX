using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class SQLTaskInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAnswerTestAnswer");

            migrationBuilder.AddColumn<Guid>(
                name: "TestSettingsId",
                table: "ThemeTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Tests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "FantomName",
                table: "TestAnswers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ClientConnectionLog",
                table: "TestAnswers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "DataRows",
                table: "Tasks",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DatabaseType",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsFailedCheck",
                table: "TaskAnswers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                table: "TaskAnswers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "TaskAnswers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TestAnswerId",
                table: "TaskAnswers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TestSettingsId",
                table: "Groups",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SqlQueryInstall = table.Column<string>(type: "text", nullable: true),
                    SqlQueryCheck = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThemeTasks_TestSettingsId",
                table: "ThemeTasks",
                column: "TestSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_SettingsId",
                table: "Tests",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SettingsId",
                table: "Tasks",
                column: "SettingsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_TestAnswerId",
                table: "TaskAnswers",
                column: "TestAnswerId");

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
                name: "FK_TaskAnswers_TestAnswers_TestAnswerId",
                table: "TaskAnswers",
                column: "TestAnswerId",
                principalTable: "TestAnswers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskSettings_SettingsId",
                table: "Tasks",
                column: "SettingsId",
                principalTable: "TaskSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestSettings_SettingsId",
                table: "Tests",
                column: "SettingsId",
                principalTable: "TestSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThemeTasks_TestSettings_TestSettingsId",
                table: "ThemeTasks",
                column: "TestSettingsId",
                principalTable: "TestSettings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_TestSettings_TestSettingsId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswers_TestAnswers_TestAnswerId",
                table: "TaskAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskSettings_SettingsId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestSettings_SettingsId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_ThemeTasks_TestSettings_TestSettingsId",
                table: "ThemeTasks");

            migrationBuilder.DropTable(
                name: "TaskSettings");

            migrationBuilder.DropTable(
                name: "TestSettings");

            migrationBuilder.DropIndex(
                name: "IX_ThemeTasks_TestSettingsId",
                table: "ThemeTasks");

            migrationBuilder.DropIndex(
                name: "IX_Tests_SettingsId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SettingsId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskAnswers_TestAnswerId",
                table: "TaskAnswers");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TestSettingsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TestSettingsId",
                table: "ThemeTasks");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "ClientConnectionLog",
                table: "TestAnswers");

            migrationBuilder.DropColumn(
                name: "DataRows",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DatabaseType",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IsFailedCheck",
                table: "TaskAnswers");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                table: "TaskAnswers");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "TaskAnswers");

            migrationBuilder.DropColumn(
                name: "TestAnswerId",
                table: "TaskAnswers");

            migrationBuilder.DropColumn(
                name: "TestSettingsId",
                table: "Groups");

            migrationBuilder.AlterColumn<string>(
                name: "FantomName",
                table: "TestAnswers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TaskAnswerTestAnswer",
                columns: table => new
                {
                    TaskAnswersId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestAnswerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAnswerTestAnswer", x => new { x.TaskAnswersId, x.TestAnswerId });
                    table.ForeignKey(
                        name: "FK_TaskAnswerTestAnswer_TaskAnswers_TaskAnswersId",
                        column: x => x.TaskAnswersId,
                        principalTable: "TaskAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAnswerTestAnswer_TestAnswers_TestAnswerId",
                        column: x => x.TestAnswerId,
                        principalTable: "TestAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswerTestAnswer_TestAnswerId",
                table: "TaskAnswerTestAnswer",
                column: "TestAnswerId");
        }
    }
}
