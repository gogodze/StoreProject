using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.db.migrations
{
    /// <inheritdoc />
    public partial class addedaddressasseperateproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    AddressLine1 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AddressLine2 = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "address");

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
        }
    }
}
