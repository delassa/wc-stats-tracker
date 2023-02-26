using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCStatsTracker.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlagString = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WCRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BossesKilled = table.Column<int>(type: "INTEGER", nullable: false),
                    CharactersFound = table.Column<int>(type: "INTEGER", nullable: false),
                    ChecksDone = table.Column<int>(type: "INTEGER", nullable: false),
                    ChestsOpened = table.Column<int>(type: "INTEGER", nullable: false),
                    DidKTSkip = table.Column<bool>(type: "INTEGER", nullable: false),
                    DragonsKilled = table.Column<int>(type: "INTEGER", nullable: false),
                    EspersFound = table.Column<int>(type: "INTEGER", nullable: false),
                    FlagId = table.Column<int>(type: "INTEGER", nullable: false),
                    RunLength = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Seed = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WCRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WCRuns_Flags_FlagId",
                        column: x => x.FlagId,
                        principalTable: "Flags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WCRuns_FlagId",
                table: "WCRuns",
                column: "FlagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WCRuns");

            migrationBuilder.DropTable(
                name: "Flags");
        }
    }
}
