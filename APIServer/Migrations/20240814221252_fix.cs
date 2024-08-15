using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TaskAnswerId",
                table: "VariableAnswers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VariableAnswers_TaskAnswerId",
                table: "VariableAnswers",
                column: "TaskAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableAnswers_TaskAnswers_TaskAnswerId",
                table: "VariableAnswers",
                column: "TaskAnswerId",
                principalTable: "TaskAnswers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariableAnswers_TaskAnswers_TaskAnswerId",
                table: "VariableAnswers");

            migrationBuilder.DropIndex(
                name: "IX_VariableAnswers_TaskAnswerId",
                table: "VariableAnswers");

            migrationBuilder.DropColumn(
                name: "TaskAnswerId",
                table: "VariableAnswers");
        }
    }
}
