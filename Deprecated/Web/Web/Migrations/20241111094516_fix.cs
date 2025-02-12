using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupCreatorId = table.Column<string>(type: "text", nullable: false),
                    GroupName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_AspNetUsers_GroupCreatorId",
                        column: x => x.GroupCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: false),
                    Question = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    InteractionType = table.Column<int>(type: "integer", nullable: false),
                    DatabaseType = table.Column<int>(type: "integer", nullable: true),
                    DataRows = table.Column<List<string>>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserTestSettings",
                columns: table => new
                {
                    TestSettingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestUsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTestSettings", x => new { x.TestSettingsId, x.TestUsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTestSettings_AspNetUsers_TestUsersId",
                        column: x => x.TestUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTestSettings_TestSettings_TestSettingsId",
                        column: x => x.TestSettingsId,
                        principalTable: "TestSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: false),
                    SettingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tests_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tests_TestSettings_SettingsId",
                        column: x => x.SettingsId,
                        principalTable: "TestSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "ApplicationUserUserGroup",
                columns: table => new
                {
                    StudentsId = table.Column<string>(type: "text", nullable: false),
                    UserGroupsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserUserGroup", x => new { x.StudentsId, x.UserGroupsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserUserGroup_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserUserGroup_Groups_UserGroupsId",
                        column: x => x.UserGroupsId,
                        principalTable: "Groups",
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
                    table.ForeignKey(
                        name: "FK_TaskSettings_Tasks_Id",
                        column: x => x.Id,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThemeTaskUniqueTask",
                columns: table => new
                {
                    ThematicsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThemeTask = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeTaskUniqueTask", x => new { x.ThematicsId, x.ThemeTask });
                    table.ForeignKey(
                        name: "FK_ThemeTaskUniqueTask_Tasks_ThemeTask",
                        column: x => x.ThemeTask,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThemeTaskUniqueTask_ThemeTasks_ThematicsId",
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
                    UniqueTaskId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariableAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariableAnswers_Tasks_UniqueTaskId",
                        column: x => x.UniqueTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<string>(type: "text", nullable: false),
                    AnsweredTestId = table.Column<Guid>(type: "uuid", nullable: true),
                    PassingDate = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    Score = table.Column<double>(type: "double precision", nullable: false),
                    FantomName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ClientConnectionLog = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAnswers_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestAnswers_Tests_AnsweredTestId",
                        column: x => x.AnsweredTestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UniqueTaskUniqueTest",
                columns: table => new
                {
                    TasksId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueTaskUniqueTest", x => new { x.TasksId, x.TestsId });
                    table.ForeignKey(
                        name: "FK_UniqueTaskUniqueTest_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UniqueTaskUniqueTest_Tests_TestsId",
                        column: x => x.TestsId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<string>(type: "text", nullable: false),
                    AnsweredTaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    TestAnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    StringAnswer = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Result = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsFailedCheck = table.Column<bool>(type: "boolean", nullable: false),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    IsCheckEnded = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAnswers_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAnswers_Tasks_AnsweredTaskId",
                        column: x => x.AnsweredTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskAnswers_TestAnswers_TestAnswerId",
                        column: x => x.TestAnswerId,
                        principalTable: "TestAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTestSettings_TestUsersId",
                table: "ApplicationUserTestSettings",
                column: "TestUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserUserGroup_UserGroupsId",
                table: "ApplicationUserUserGroup",
                column: "UserGroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupCreatorId",
                table: "Groups",
                column: "GroupCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_AnsweredTaskId",
                table: "TaskAnswers",
                column: "AnsweredTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_StudentId",
                table: "TaskAnswers",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswers_TestAnswerId",
                table: "TaskAnswers",
                column: "TestAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswerVariableAnswer_TaskAnswerId",
                table: "TaskAnswerVariableAnswer",
                column: "TaskAnswerId");

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
                name: "IX_Tests_SettingsId",
                table: "Tests",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSettingsThemeTask_TestSettingsId",
                table: "TestSettingsThemeTask",
                column: "TestSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_TestSettingsUserGroup_TestSettingsId",
                table: "TestSettingsUserGroup",
                column: "TestSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ThemeTaskUniqueTask_ThemeTask",
                table: "ThemeTaskUniqueTask",
                column: "ThemeTask");

            migrationBuilder.CreateIndex(
                name: "IX_UniqueTaskUniqueTest_TestsId",
                table: "UniqueTaskUniqueTest",
                column: "TestsId");

            migrationBuilder.CreateIndex(
                name: "IX_VariableAnswers_UniqueTaskId",
                table: "VariableAnswers",
                column: "UniqueTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTestSettings");

            migrationBuilder.DropTable(
                name: "ApplicationUserUserGroup");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "TaskAnswerVariableAnswer");

            migrationBuilder.DropTable(
                name: "TaskSettings");

            migrationBuilder.DropTable(
                name: "TestSettingsThemeTask");

            migrationBuilder.DropTable(
                name: "TestSettingsUserGroup");

            migrationBuilder.DropTable(
                name: "ThemeTaskUniqueTask");

            migrationBuilder.DropTable(
                name: "UniqueTaskUniqueTest");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "TaskAnswers");

            migrationBuilder.DropTable(
                name: "VariableAnswers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "ThemeTasks");

            migrationBuilder.DropTable(
                name: "TestAnswers");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TestSettings");
        }
    }
}
