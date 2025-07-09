using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarProject.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LastUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // String rolleri integer'a dönüştür
            migrationBuilder.Sql("UPDATE [Users] SET [Role] = 1 WHERE [Role] = 'Admin'");
            migrationBuilder.Sql("UPDATE [Users] SET [Role] = 0 WHERE [Role] = 'User'");
            migrationBuilder.Sql("UPDATE [Users] SET [Role] = 0 WHERE [Role] IS NULL");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "Customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "Customer",
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
