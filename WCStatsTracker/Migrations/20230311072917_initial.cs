using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WCStatsTracker.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbilitiesProtected",
                columns: table => new
                {
                    AbilityId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilitiesProtected", x => x.AbilityId);
                });

            migrationBuilder.CreateTable(
                name: "CharactersProtected",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharactersProtected", x => x.CharacterId);
                });

            migrationBuilder.CreateTable(
                name: "Flags",
                columns: table => new
                {
                    FlagId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlagString = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flags", x => x.FlagId);
                });

            migrationBuilder.CreateTable(
                name: "WCRuns",
                columns: table => new
                {
                    WcRunId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    Seed = table.Column<string>(type: "TEXT", nullable: false),
                    DateRan = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WCRuns", x => x.WcRunId);
                    table.ForeignKey(
                        name: "FK_WCRuns_Flags_FlagId",
                        column: x => x.FlagId,
                        principalTable: "Flags",
                        principalColumn: "FlagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbilityWcRun",
                columns: table => new
                {
                    AbilitiesAbilityId = table.Column<int>(type: "INTEGER", nullable: false),
                    RunsWcRunId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbilityWcRun", x => new { x.AbilitiesAbilityId, x.RunsWcRunId });
                    table.ForeignKey(
                        name: "FK_AbilityWcRun_AbilitiesProtected_AbilitiesAbilityId",
                        column: x => x.AbilitiesAbilityId,
                        principalTable: "AbilitiesProtected",
                        principalColumn: "AbilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbilityWcRun_WCRuns_RunsWcRunId",
                        column: x => x.RunsWcRunId,
                        principalTable: "WCRuns",
                        principalColumn: "WcRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterWcRun",
                columns: table => new
                {
                    CharactersCharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    RunsWcRunId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterWcRun", x => new { x.CharactersCharacterId, x.RunsWcRunId });
                    table.ForeignKey(
                        name: "FK_CharacterWcRun_CharactersProtected_CharactersCharacterId",
                        column: x => x.CharactersCharacterId,
                        principalTable: "CharactersProtected",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterWcRun_WCRuns_RunsWcRunId",
                        column: x => x.RunsWcRunId,
                        principalTable: "WCRuns",
                        principalColumn: "WcRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AbilitiesProtected",
                columns: new[] { "AbilityId", "Name" },
                values: new object[,]
                {
                    { 1, "Capture" },
                    { 2, "Control" },
                    { 3, "GP Rain" },
                    { 4, "Dance" },
                    { 5, "Health" },
                    { 6, "Jump" },
                    { 7, "Lore" },
                    { 8, "Morph" },
                    { 9, "Rage" },
                    { 10, "Runic" },
                    { 11, "Sketch" },
                    { 12, "Slot" },
                    { 13, "Steal" },
                    { 14, "SwdTech" },
                    { 15, "Throw" },
                    { 16, "Tools" },
                    { 17, "X Magic" },
                    { 18, "Shock" },
                    { 19, "MagiTek" },
                    { 20, "Possess" },
                    { 21, "Blitz" }
                });

            migrationBuilder.InsertData(
                table: "CharactersProtected",
                columns: new[] { "CharacterId", "Name" },
                values: new object[,]
                {
                    { 1, "Terra" },
                    { 2, "Locke" },
                    { 3, "Cyan" },
                    { 4, "Shadow" },
                    { 5, "Edgar" },
                    { 6, "Sabin" },
                    { 7, "Celes" },
                    { 8, "Strage" },
                    { 9, "Relm" },
                    { 10, "Setzer" },
                    { 11, "Mog" },
                    { 12, "Gau" },
                    { 13, "Gogo" },
                    { 14, "Umaro" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbilityWcRun_RunsWcRunId",
                table: "AbilityWcRun",
                column: "RunsWcRunId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterWcRun_RunsWcRunId",
                table: "CharacterWcRun",
                column: "RunsWcRunId");

            migrationBuilder.CreateIndex(
                name: "IX_WCRuns_FlagId",
                table: "WCRuns",
                column: "FlagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbilityWcRun");

            migrationBuilder.DropTable(
                name: "CharacterWcRun");

            migrationBuilder.DropTable(
                name: "AbilitiesProtected");

            migrationBuilder.DropTable(
                name: "CharactersProtected");

            migrationBuilder.DropTable(
                name: "WCRuns");

            migrationBuilder.DropTable(
                name: "Flags");
        }
    }
}
