using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedDtos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardHolderName",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumberMasked",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpireMonth",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpireYear",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstallmentCount",
                table: "Payments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardHolderName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardNumberMasked",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ExpireMonth",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ExpireYear",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "InstallmentCount",
                table: "Payments");
        }
    }
}
