using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIServer.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThemeTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Theme = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccessFlags = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupCreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Users_GroupCreatorId",
                        column: x => x.GroupCreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Question = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    InteractionType = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TestName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserUserGroup",
                columns: table => new
                {
                    StudentsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGroupsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUserGroup", x => new { x.StudentsId, x.UserGroupsId });
                    table.ForeignKey(
                        name: "FK_UserUserGroup_Groups_UserGroupsId",
                        column: x => x.UserGroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUserGroup_Users_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    AnsweredTaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    StringAnswer = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAnswers_Tasks_AnsweredTaskId",
                        column: x => x.AnsweredTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskAnswers_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskThemeTask",
                columns: table => new
                {
                    ThematicsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThemeTask = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskThemeTask", x => new { x.ThematicsId, x.ThemeTask });
                    table.ForeignKey(
                        name: "FK_TaskThemeTask_Tasks_ThemeTask",
                        column: x => x.ThemeTask,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskThemeTask_ThemeTasks_ThematicsId",
                        column: x => x.ThematicsId,
                        principalTable: "ThemeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariableAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StringAnswer = table.Column<string>(type: "text", nullable: true),
                    Truthful = table.Column<bool>(type: "boolean", nullable: true),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariableAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariableAnswers_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskTest",
                columns: table => new
                {
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTest", x => new { x.TasksId, x.TestsId });
                    table.ForeignKey(
                        name: "FK_TaskTest_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTest_Tests_TestsId",
                        column: x => x.TestsId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    AnsweredTestId = table.Column<Guid>(type: "uuid", nullable: true),
                    PassingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    FantomName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAnswers_Tests_AnsweredTestId",
                        column: x => x.AnsweredTestId,
                        principalTable: "Tests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestAnswers_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskAnswerVariableAnswer",
                columns: table => new
                {
                    MarkedVariablesId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskAnswerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAnswerVariableAnswer", x => new { x.MarkedVariablesId, x.TaskAnswerId });
                    table.ForeignKey(
                        name: "FK_TaskAnswerVariableAnswer_TaskAnswers_TaskAnswerId",
                        column: x => x.TaskAnswerId,
                        principalTable: "TaskAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAnswerVariableAnswer_VariableAnswers_MarkedVariablesId",
                        column: x => x.MarkedVariablesId,
                        principalTable: "VariableAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Groups_GroupCreatorId",
                table: "Groups",
                column: "GroupCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswerTestAnswer_TestAnswerId",
                table: "TaskAnswerTestAnswer",
                column: "TestAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswerVariableAnswer_TaskAnswerId",
                table: "TaskAnswerVariableAnswer",
                column: "TaskAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_AnsweredTaskId",
                table: "TaskAnswers",
                column: "AnsweredTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_StudentId",
                table: "TaskAnswers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTest_TestsId",
                table: "TaskTest",
                column: "TestsId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskThemeTask_ThemeTask",
                table: "TaskThemeTask",
                column: "ThemeTask");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatorId",
                table: "Tasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_AnsweredTestId",
                table: "TestAnswers",
                column: "AnsweredTestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAnswers_StudentId",
                table: "TestAnswers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_CreatorId",
                table: "Tests",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUserGroup_UserGroupsId",
                table: "UserUserGroup",
                column: "UserGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VariableAnswers_TaskId",
                table: "VariableAnswers",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAnswerTestAnswer");

            migrationBuilder.DropTable(
                name: "TaskAnswerVariableAnswer");

            migrationBuilder.DropTable(
                name: "TaskTest");

            migrationBuilder.DropTable(
                name: "TaskThemeTask");

            migrationBuilder.DropTable(
                name: "UserUserGroup");

            migrationBuilder.DropTable(
                name: "TestAnswers");

            migrationBuilder.DropTable(
                name: "TaskAnswers");

            migrationBuilder.DropTable(
                name: "VariableAnswers");

            migrationBuilder.DropTable(
                name: "ThemeTasks");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
