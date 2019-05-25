using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMovies.DAL.Migrations
{
    public partial class Add_rate_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RateType = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(type: "decimal(12,10)", nullable: false),
                    MovieId = table.Column<Guid>(nullable: false),
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rates_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rates_MovieId",
                table: "Rates",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_PersonId",
                table: "Rates",
                column: "PersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Movies",
                type: "decimal(12,10)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
