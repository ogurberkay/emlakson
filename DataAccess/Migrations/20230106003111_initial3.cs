using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Images_ImageFileImageId",
                table: "Adverts");

            migrationBuilder.AlterColumn<int>(
                name: "ImageFileImageId",
                table: "Adverts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "fd047cb9-9985-4fd7-ad8b-f8352414b446");

            migrationBuilder.UpdateData(
                table: "IdentityRoleEntity",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "89eb9d32-13e7-4f91-8f34-5ec59cbeddc9");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Images_ImageFileImageId",
                table: "Adverts",
                column: "ImageFileImageId",
                principalTable: "Images",
                principalColumn: "ImageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Images_ImageFileImageId",
                table: "Adverts");

            migrationBuilder.AlterColumn<int>(
                name: "ImageFileImageId",
                table: "Adverts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Images_ImageFileImageId",
                table: "Adverts",
                column: "ImageFileImageId",
                principalTable: "Images",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
