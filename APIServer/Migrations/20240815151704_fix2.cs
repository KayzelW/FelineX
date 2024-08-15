using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswerVariableAnswers_TaskAnswers_TaskAnswerId",
                table: "TaskAnswerVariableAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswerVariableAnswers_VariableAnswers_MarkedVariableAns~",
                table: "TaskAnswerVariableAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableAnswers_TaskAnswers_TaskAnswerId",
                table: "VariableAnswers");

            migrationBuilder.DropIndex(
                name: "IX_VariableAnswers_TaskAnswerId",
                table: "VariableAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAnswerVariableAnswers",
                table: "TaskAnswerVariableAnswers");

            migrationBuilder.DropIndex(
                name: "IX_TaskAnswerVariableAnswers_MarkedVariableAnswerId",
                table: "TaskAnswerVariableAnswers");

            migrationBuilder.DropColumn(
                name: "TaskAnswerId",
                table: "VariableAnswers");

            migrationBuilder.RenameTable(
                name: "TaskAnswerVariableAnswers",
                newName: "TaskAnswerVariableAnswer");

            migrationBuilder.RenameColumn(
                name: "MarkedVariableAnswerId",
                table: "TaskAnswerVariableAnswer",
                newName: "MarkedVariablesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAnswerVariableAnswer",
                table: "TaskAnswerVariableAnswer",
                columns: new[] { "MarkedVariablesId", "TaskAnswerId" });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswerVariableAnswer_TaskAnswerId",
                table: "TaskAnswerVariableAnswer",
                column: "TaskAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswerVariableAnswer_TaskAnswers_TaskAnswerId",
                table: "TaskAnswerVariableAnswer",
                column: "TaskAnswerId",
                principalTable: "TaskAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswerVariableAnswer_VariableAnswers_MarkedVariablesId",
                table: "TaskAnswerVariableAnswer",
                column: "MarkedVariablesId",
                principalTable: "VariableAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswerVariableAnswer_TaskAnswers_TaskAnswerId",
                table: "TaskAnswerVariableAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswerVariableAnswer_VariableAnswers_MarkedVariablesId",
                table: "TaskAnswerVariableAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskAnswerVariableAnswer",
                table: "TaskAnswerVariableAnswer");

            migrationBuilder.DropIndex(
                name: "IX_TaskAnswerVariableAnswer_TaskAnswerId",
                table: "TaskAnswerVariableAnswer");

            migrationBuilder.RenameTable(
                name: "TaskAnswerVariableAnswer",
                newName: "TaskAnswerVariableAnswers");

            migrationBuilder.RenameColumn(
                name: "MarkedVariablesId",
                table: "TaskAnswerVariableAnswers",
                newName: "MarkedVariableAnswerId");

            migrationBuilder.AddColumn<Guid>(
                name: "TaskAnswerId",
                table: "VariableAnswers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskAnswerVariableAnswers",
                table: "TaskAnswerVariableAnswers",
                columns: new[] { "TaskAnswerId", "MarkedVariableAnswerId" });

            migrationBuilder.CreateIndex(
                name: "IX_VariableAnswers_TaskAnswerId",
                table: "VariableAnswers",
                column: "TaskAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswerVariableAnswers_MarkedVariableAnswerId",
                table: "TaskAnswerVariableAnswers",
                column: "MarkedVariableAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswerVariableAnswers_TaskAnswers_TaskAnswerId",
                table: "TaskAnswerVariableAnswers",
                column: "TaskAnswerId",
                principalTable: "TaskAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswerVariableAnswers_VariableAnswers_MarkedVariableAns~",
                table: "TaskAnswerVariableAnswers",
                column: "MarkedVariableAnswerId",
                principalTable: "VariableAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariableAnswers_TaskAnswers_TaskAnswerId",
                table: "VariableAnswers",
                column: "TaskAnswerId",
                principalTable: "TaskAnswers",
                principalColumn: "Id");
        }
    }
}
