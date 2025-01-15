using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Migrations
{
    /// <inheritdoc />
    public partial class AddLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tests_TestSettings_SettingsId",
                table: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Tests_SettingsId",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Tests");

            migrationBuilder.AddColumn<Guid>(
                name: "UniqueTestId",
                table: "TestSettings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TestId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestSettingsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestLinks_TestSettings_TestSettingsId",
                        column: x => x.TestSettingsId,
                        principalTable: "TestSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestLinks_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestSettings_UniqueTestId",
                table: "TestSettings",
                column: "UniqueTestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestLinks_TestId",
                table: "TestLinks",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestLinks_TestSettingsId",
                table: "TestLinks",
                column: "TestSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestSettings_Tests_UniqueTestId",
                table: "TestSettings",
                column: "UniqueTestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestSettings_Tests_UniqueTestId",
                table: "TestSettings");

            migrationBuilder.DropTable(
                name: "TestLinks");

            migrationBuilder.DropIndex(
                name: "IX_TestSettings_UniqueTestId",
                table: "TestSettings");

            migrationBuilder.DropColumn(
                name: "UniqueTestId",
                table: "TestSettings");

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Tests",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tests_SettingsId",
                table: "Tests",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tests_TestSettings_SettingsId",
                table: "Tests",
                column: "SettingsId",
                principalTable: "TestSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
