using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ExtraAttributess");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ExtraAttributess");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExtraAttributess");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ExtraAttributess");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ExtraAttributess");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2fb92c79-4f23-4612-aeab-0f710ca894d2");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "acea6669-04a8-4e24-86dd-2594280aceed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "ExtraAttributess",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ExtraAttributess",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExtraAttributess",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ExtraAttributess",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ExtraAttributess",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7388fde9-301b-4283-99c0-e3700ccbe536");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "5bea5fd4-9226-4ad4-b600-ac84116b4c5b");
        }
    }
}
