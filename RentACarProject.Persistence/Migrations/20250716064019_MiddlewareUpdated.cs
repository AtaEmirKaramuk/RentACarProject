using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MiddlewareUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElapsedMilliseconds",
                table: "Logs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExceptionMessage",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogLevel",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElapsedMilliseconds",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "ExceptionMessage",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "LogLevel",
                table: "Logs");
        }
    }
}
