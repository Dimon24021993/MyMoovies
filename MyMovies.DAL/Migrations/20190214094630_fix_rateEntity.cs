using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMovies.DAL.Migrations
{
    public partial class fix_rateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Persons_PersonId",
                table: "Rates");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Rates",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_PersonId",
                table: "Rates",
                newName: "IX_Rates_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Users_UserId",
                table: "Rates",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_Users_UserId",
                table: "Rates");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Rates",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Rates_UserId",
                table: "Rates",
                newName: "IX_Rates_PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_Persons_PersonId",
                table: "Rates",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
