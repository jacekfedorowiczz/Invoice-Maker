using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceMaker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Vendor_FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Vendor_LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Vendor_PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Vendor_TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendor_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendee_FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Vendee_LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Vendee_PhoneNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    Vendee_TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Vendee_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendee_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendee_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Vendee_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(28)", maxLength: 28, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(7,2)", precision: 7, scale: 2, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_InvoiceId",
                table: "Items",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
