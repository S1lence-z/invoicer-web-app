using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Pgsql
{
    /// <inheritdoc />
    public partial class OptionalMethodsPgsql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Invoice",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "BankTransfer");

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryMethod",
                table: "Invoice",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "PersonalPickUp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Invoice",
                type: "text",
                nullable: false,
                defaultValue: "BankTransfer",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeliveryMethod",
                table: "Invoice",
                type: "text",
                nullable: false,
                defaultValue: "PersonalPickUp",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
