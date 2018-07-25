using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserDictionary.Migrations
{
    public partial class Mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Url = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Abstract = table.Column<string>(nullable: true),
                    LastReadTime = table.Column<DateTime>(nullable: false),
                    LastReadPlace = table.Column<string>(nullable: true),
                    RemainWords = table.Column<string>(nullable: true),
                    Page = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Translation = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    YesorNo = table.Column<int>(nullable: false),
                    Phonetic = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dictionaries",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TotalWordsNumber = table.Column<int>(nullable: false),
                    WordId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dictionaries_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryWords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WordId = table.Column<string>(nullable: true),
                    DictionaryId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DictionaryWords_Dictionaries_DictionaryId",
                        column: x => x.DictionaryId,
                        principalTable: "Dictionaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DictionaryWords_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_WordId",
                table: "Dictionaries",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryWords_DictionaryId",
                table: "DictionaryWords",
                column: "DictionaryId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryWords_WordId",
                table: "DictionaryWords",
                column: "WordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryWords");

            migrationBuilder.DropTable(
                name: "Passages");

            migrationBuilder.DropTable(
                name: "Dictionaries");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
