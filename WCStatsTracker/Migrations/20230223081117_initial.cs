using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WCStatsTracker.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
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
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FlagString = table.Column<string>(type: "TEXT", nullable: false)
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
                    RunLength = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    CharactersFound = table.Column<int>(type: "INTEGER", nullable: false),
                    EspersFound = table.Column<int>(type: "INTEGER", nullable: false),
                    DragonsKilled = table.Column<int>(type: "INTEGER", nullable: false),
                    BossesKilled = table.Column<int>(type: "INTEGER", nullable: false),
                    ChecksDone = table.Column<int>(type: "INTEGER", nullable: false),
                    ChestsOpened = table.Column<int>(type: "INTEGER", nullable: false),
                    DidKTSkip = table.Column<bool>(type: "INTEGER", nullable: false),
                    FlagId = table.Column<int>(type: "INTEGER", nullable: true),
                    Seed = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WCRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WCRuns_Flags_FlagId",
                        column: x => x.FlagId,
                        principalTable: "Flags",
                        principalColumn: "Id");
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
