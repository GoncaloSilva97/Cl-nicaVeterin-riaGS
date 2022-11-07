using Microsoft.EntityFrameworkCore.Migrations;

namespace VeterinaryClinicGS.Migrations
{
    public partial class r : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_ServiceTypes_ServiceTypeId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTypeId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_ServiceTypes_ServiceTypeId",
                table: "Rooms",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_ServiceTypes_ServiceTypeId",
                table: "Rooms");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTypeId",
                table: "Rooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_ServiceTypes_ServiceTypeId",
                table: "Rooms",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
