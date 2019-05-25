using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMovies.DAL.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Movies",
                type: "decimal(12,10)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "Float(5)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                table: "Movies",
                type: "Float(5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,10)");
        }
    }
}
