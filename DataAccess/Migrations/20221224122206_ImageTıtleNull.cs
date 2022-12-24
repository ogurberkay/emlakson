using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class ImageTıtleNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Images",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5b10faaa-84f5-465f-9bac-cf1c3a3755bb");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "59f6f056-3eca-439a-8dd0-470030c9fa1b");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Images",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "62732f88-6aba-4daf-a18f-b1f92a38f16f");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "01729883-65c8-413e-9750-30bbcb9408aa");
        }
    }
}
