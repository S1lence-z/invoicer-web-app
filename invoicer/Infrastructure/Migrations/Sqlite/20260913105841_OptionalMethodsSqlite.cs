using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class OptionalMethodsSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Invoice",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "BankTransfer");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryMethod",
                table: "Invoice",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "PersonalPickUp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Invoice",
                type: "TEXT",
                nullable: false,
                defaultValue: "BankTransfer",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryMethod",
                table: "Invoice",
                type: "TEXT",
                nullable: false,
                defaultValue: "PersonalPickUp",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
