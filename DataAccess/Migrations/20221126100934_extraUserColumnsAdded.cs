using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class extraUserColumnsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByImpId",
                table: "Adverts");

            migrationBuilder.RenameColumn(
                name: "ModifiedByImpId",
                table: "Adverts",
                newName: "CreatedById");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                schema: "aid",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                schema: "aid",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                schema: "aid",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "aid",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                schema: "aid",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "aid",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "aid",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "aid",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b9b7272c-46fc-4f89-8d64-610c8fd78d36");

            migrationBuilder.UpdateData(
                schema: "aid",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "50c7f023-6506-42b1-b9b5-13c0aab7c79d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "aid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                schema: "aid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DateModified",
                schema: "aid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "aid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                schema: "aid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "aid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "aid",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Adverts",
                newName: "ModifiedByImpId");

            migrationBuilder.AddColumn<int>(
                name: "CreatedByImpId",
                table: "Adverts",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "aid",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e25d5bf5-1042-453b-9a97-f6507ab619ad");

            migrationBuilder.UpdateData(
                schema: "aid",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "8ed35e80-953a-4896-954b-648bfe1d30d4");
        }
    }
}
