using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskSettings_SettingsId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_SettingsId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Tasks");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskId",
                table: "TaskSettings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TaskSettings_TaskId",
                table: "TaskSettings",
                column: "TaskId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskSettings_Tasks_TaskId",
                table: "TaskSettings",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskSettings_Tasks_TaskId",
                table: "TaskSettings");

            migrationBuilder.DropIndex(
                name: "IX_TaskSettings_TaskId",
                table: "TaskSettings");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "TaskSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_SettingsId",
                table: "Tasks",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskSettings_SettingsId",
                table: "Tasks",
                column: "SettingsId",
                principalTable: "TaskSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
