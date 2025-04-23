using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_product",
                table: "order_product");

            migrationBuilder.DropIndex(
                name: "IX_order_product_OrderId",
                table: "order_product");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine1",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine2",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_ZipCode",
                table: "users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOrderFinished",
                table: "orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOrdered",
                table: "orders",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "orders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_product",
                table: "order_product",
                columns: new[] { "OrderId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_order_product_ProductId",
                table: "order_product",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_order_product_products_ProductId",
                table: "order_product",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_product_products_ProductId",
                table: "order_product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_order_product",
                table: "order_product");

            migrationBuilder.DropIndex(
                name: "IX_order_product_ProductId",
                table: "order_product");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine1",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine2",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_State",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                table: "users");

            migrationBuilder.DropColumn(
                name: "DateOrderFinished",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DateOrdered",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "orders");

            migrationBuilder.AddPrimaryKey(
                name: "PK_order_product",
                table: "order_product",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCode = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_address_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_details",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "TEXT", nullable: false),
                    DateOrderFinished = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateOrdered = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_details", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_order_details_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_product_OrderId",
                table: "order_product",
                column: "OrderId");
        }
    }
}
