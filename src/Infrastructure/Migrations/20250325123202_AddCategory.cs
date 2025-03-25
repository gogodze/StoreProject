using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "users",
                newName: "ProfilePicture");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "products",
                newName: "PreviewImage");

            migrationBuilder.RenameColumn(
                name: "DateFinished",
                table: "orders",
                newName: "DateOrdered");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "orders",
                newName: "DateOrderFinished");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_products_CategoryId",
                table: "products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_category_CategoryId",
                table: "products",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_category_CategoryId",
                table: "products");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropIndex(
                name: "IX_products_CategoryId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "ProfilePicture",
                table: "users",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "PreviewImage",
                table: "products",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "DateOrdered",
                table: "orders",
                newName: "DateFinished");

            migrationBuilder.RenameColumn(
                name: "DateOrderFinished",
                table: "orders",
                newName: "DateCreated");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateAdded",
                table: "products",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
