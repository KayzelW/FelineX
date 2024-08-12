using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_GroupCreatorId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswers_Tasks_AnsweredTaskId",
                table: "TaskAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswers_TestAnswers_TestAnswerId",
                table: "TaskAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswers_Tests_AnsweredTestId",
                table: "TestAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswers_Users_StudentId",
                table: "TestAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Users_CreatorId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableAnswers_Tasks_TaskId",
                table: "VariableAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_GroupCreatorId",
                table: "Groups",
                column: "GroupCreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswers_Tasks_AnsweredTaskId",
                table: "TaskAnswers",
                column: "AnsweredTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswers_TestAnswers_TestAnswerId",
                table: "TaskAnswers",
                column: "TestAnswerId",
                principalTable: "TestAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswers_Tests_AnsweredTestId",
                table: "TestAnswers",
                column: "AnsweredTestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswers_Users_StudentId",
                table: "TestAnswers",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Users_CreatorId",
                table: "Tests",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VariableAnswers_Tasks_TaskId",
                table: "VariableAnswers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_GroupCreatorId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswers_Tasks_AnsweredTaskId",
                table: "TaskAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswers_TestAnswers_TestAnswerId",
                table: "TaskAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswers_Tests_AnsweredTestId",
                table: "TestAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestAnswers_Users_StudentId",
                table: "TestAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tests_Users_CreatorId",
                table: "Tests");

            migrationBuilder.DropForeignKey(
                name: "FK_VariableAnswers_Tasks_TaskId",
                table: "VariableAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_GroupCreatorId",
                table: "Groups",
                column: "GroupCreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswers_Tasks_AnsweredTaskId",
                table: "TaskAnswers",
                column: "AnsweredTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswers_TestAnswers_TestAnswerId",
                table: "TaskAnswers",
                column: "TestAnswerId",
                principalTable: "TestAnswers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswers_Tests_AnsweredTestId",
                table: "TestAnswers",
                column: "AnsweredTestId",
                principalTable: "Tests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TestAnswers_Users_StudentId",
                table: "TestAnswers",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_Users_CreatorId",
                table: "Tests",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VariableAnswers_Tasks_TaskId",
                table: "VariableAnswers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
