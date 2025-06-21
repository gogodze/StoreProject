using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeEverythingEncryptedQueryable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine1ShadowHash",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine2ShadowHash",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_CityShadowHash",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_CountryShadowHash",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_StateShadowHash",
                table: "users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullNameShadowHash",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HashedPasswordShadowHash",
                table: "users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshTokenShadowHash",
                table: "users",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_AddressLine1ShadowHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine2ShadowHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_CityShadowHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_CountryShadowHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Address_StateShadowHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "FullNameShadowHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "HashedPasswordShadowHash",
                table: "users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenShadowHash",
                table: "users");
        }
    }
}
