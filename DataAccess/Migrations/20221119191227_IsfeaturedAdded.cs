using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class IsfeaturedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5c7b1628-9b73-47cf-8356-7cece3686644");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90572239-4f5c-419c-aa54-83067563547f");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Adverts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4914f822-331e-4355-9665-74724e0f83d6", "b0b74c96-2f0b-4c7c-a120-08a6832364f1", "SuperAdmin", "SUPER_ADMIN" },
                    { "ce456b8c-7541-419c-8511-f792365ddedc", "5b09fb86-eefa-44f0-8682-aae453065a88", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4914f822-331e-4355-9665-74724e0f83d6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce456b8c-7541-419c-8511-f792365ddedc");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Adverts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5c7b1628-9b73-47cf-8356-7cece3686644", "988ed2d6-e125-4064-9f1b-d99d5ed7b65d", "Admin", "ADMIN" },
                    { "90572239-4f5c-419c-aa54-83067563547f", "4e8ae0e3-15a7-4164-b019-4292a9baab54", "SuperAdmin", "SUPER_ADMIN" }
                });
        }
    }
}
