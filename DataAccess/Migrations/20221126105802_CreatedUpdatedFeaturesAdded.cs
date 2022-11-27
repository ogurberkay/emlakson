using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class CreatedUpdatedFeaturesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateModified",
                schema: "aid",
                table: "Users",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "aid",
                table: "Users",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Adverts",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Adverts",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedById",
                table: "Adverts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 1,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 2,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModifiedById",
                value: null);

            migrationBuilder.UpdateData(
                schema: "aid",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ae634538-66a6-4ddf-9583-2a6be072e24d");

            migrationBuilder.UpdateData(
                schema: "aid",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "7399fd16-8dca-4bdd-b571-0ca051639d44");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                schema: "aid",
                table: "Users",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "aid",
                table: "Users",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Adverts",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Adverts",
                newName: "DateCreated");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedById",
                table: "Adverts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 1,
                column: "ModifiedById",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 2,
                column: "ModifiedById",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModifiedById",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModifiedById",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModifiedById",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModifiedById",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModifiedById",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Adverts",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModifiedById",
                value: 0);

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
    }
}
