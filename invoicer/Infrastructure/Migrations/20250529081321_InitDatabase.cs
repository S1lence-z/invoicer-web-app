using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Street = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    ZipCode = table.Column<int>(type: "INTEGER", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountNumber = table.Column<string>(type: "TEXT", nullable: false),
                    BankCode = table.Column<string>(type: "TEXT", nullable: false),
                    BankName = table.Column<string>(type: "TEXT", nullable: false),
                    IBAN = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NumberingScheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Prefix = table.Column<string>(type: "TEXT", nullable: false, defaultValue: ""),
                    UseSeperator = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Seperator = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "-"),
                    SequencePosition = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Start"),
                    SequencePadding = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 3),
                    InvoiceNumberYearFormat = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "FourDigit"),
                    IncludeMonth = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    ResetFrequency = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Yearly"),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberingScheme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ico = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false, defaultValue: ""),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false, defaultValue: ""),
                    BankAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    AddressId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentNumberingSchemeId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsClient = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entity_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entity_BankAccount_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entity_NumberingScheme_CurrentNumberingSchemeId",
                        column: x => x.CurrentNumberingSchemeId,
                        principalTable: "NumberingScheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntityInvoiceNumberingSchemeState",
                columns: table => new
                {
                    EntityId = table.Column<int>(type: "INTEGER", nullable: false),
                    LastSequenceNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    LastGenerationYear = table.Column<int>(type: "INTEGER", nullable: false),
                    LastGenerationMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberingSchemeId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityInvoiceNumberingSchemeState", x => x.EntityId);
                    table.ForeignKey(
                        name: "FK_EntityInvoiceNumberingSchemeState_Entity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityInvoiceNumberingSchemeState_NumberingScheme_NumberingSchemeId",
                        column: x => x.NumberingSchemeId,
                        principalTable: "NumberingScheme",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SellerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyerId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VatDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "CZK"),
                    PaymentMethod = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "BankTransfer"),
                    DeliveryMethod = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "PersonalPickUp"),
                    Status = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "Pending"),
                    NumberingSchemeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Entity_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_Entity_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Entity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_NumberingScheme_NumberingSchemeId",
                        column: x => x.NumberingSchemeId,
                        principalTable: "NumberingScheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<decimal>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    VatRate = table.Column<decimal>(type: "TEXT", nullable: false, defaultValue: 0.21m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItem_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NumberingScheme",
                columns: new[] { "Id", "IncludeMonth", "IsDefault", "Prefix", "Seperator", "SequencePadding", "UseSeperator" },
                values: new object[] { 1, true, true, "INV", "-", 3, true });

            migrationBuilder.CreateIndex(
                name: "IX_Entity_AddressId",
                table: "Entity",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_BankAccountId",
                table: "Entity",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Entity_CurrentNumberingSchemeId",
                table: "Entity",
                column: "CurrentNumberingSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityInvoiceNumberingSchemeState_NumberingSchemeId",
                table: "EntityInvoiceNumberingSchemeState",
                column: "NumberingSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_BuyerId",
                table: "Invoice",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_NumberingSchemeId",
                table: "Invoice",
                column: "NumberingSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_SellerId",
                table: "Invoice",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_InvoiceId",
                table: "InvoiceItem",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityInvoiceNumberingSchemeState");

            migrationBuilder.DropTable(
                name: "InvoiceItem");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Entity");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "BankAccount");

            migrationBuilder.DropTable(
                name: "NumberingScheme");
        }
    }
}
