using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventarni_system.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Budovy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazev = table.Column<string>(type: "TEXT", nullable: false),
                    Typ = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budovy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sklady",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazev = table.Column<string>(type: "TEXT", nullable: false),
                    BudovaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sklady", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sklady_Budovy_BudovaId",
                        column: x => x.BudovaId,
                        principalTable: "Budovy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Predmety",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazev = table.Column<string>(type: "TEXT", nullable: false),
                    Mnozstvi = table.Column<int>(type: "INTEGER", nullable: false),
                    CenaZaKus = table.Column<decimal>(type: "TEXT", nullable: false),
                    SkladId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmety", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predmety_Sklady_SkladId",
                        column: x => x.SkladId,
                        principalTable: "Sklady",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Predmety_SkladId",
                table: "Predmety",
                column: "SkladId");

            migrationBuilder.CreateIndex(
                name: "IX_Sklady_BudovaId",
                table: "Sklady",
                column: "BudovaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predmety");

            migrationBuilder.DropTable(
                name: "Sklady");

            migrationBuilder.DropTable(
                name: "Budovy");
        }
    }
}
