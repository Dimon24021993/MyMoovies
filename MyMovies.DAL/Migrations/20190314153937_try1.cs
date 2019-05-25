using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMovies.DAL.Migrations
{
    public partial class try1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Persons_PersonId",
                table: "Items");

            migrationBuilder.AlterColumn<Guid>(
                name: "PersonId",
                table: "Items",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Persons_PersonId",
                table: "Items",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Persons_PersonId",
                table: "Items");

            migrationBuilder.AlterColumn<Guid>(
                name: "PersonId",
                table: "Items",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Persons_PersonId",
                table: "Items",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
