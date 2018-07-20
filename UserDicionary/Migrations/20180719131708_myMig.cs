using Microsoft.EntityFrameworkCore.Migrations;

namespace UserDicionary.Migrations
{
    public partial class myMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customDics",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userName = table.Column<string>(nullable: true),
                    word = table.Column<string>(nullable: true),
                    translation = table.Column<string>(nullable: true),
                    classification = table.Column<int>(nullable: false),
                    yesorNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customDics", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "passages",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    url = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    remainWords = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_passages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userName = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    remainWords_1 = table.Column<string>(nullable: true),
                    remainWords_2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customDics");

            migrationBuilder.DropTable(
                name: "passages");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
