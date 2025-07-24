using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PaymentUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ReservationId",
                table: "Reservations",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "SenderIban",
                table: "Payments",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderIban",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reservations",
                newName: "ReservationId");

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "Payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
