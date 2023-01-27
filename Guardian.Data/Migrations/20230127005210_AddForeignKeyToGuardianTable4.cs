using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guardian.Data.Migrations
{
    public partial class AddForeignKeyToGuardianTable4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdResponsible",
                table: "GuardianTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "GuardianTasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 1, 26, 21, 52, 9, 910, DateTimeKind.Local).AddTicks(3025));

            migrationBuilder.CreateIndex(
                name: "IX_GuardianTasks_IdResponsible",
                table: "GuardianTasks",
                column: "IdResponsible");

            migrationBuilder.AddForeignKey(
                name: "FK_GuardianTasks_Guardians_IdResponsible",
                table: "GuardianTasks",
                column: "IdResponsible",
                principalTable: "Guardians",
                principalColumn: "Id",
                onDelete: ReferentialAction.);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuardianTasks_Guardians_IdResponsible",
                table: "GuardianTasks");

            migrationBuilder.DropIndex(
                name: "IX_GuardianTasks_IdResponsible",
                table: "GuardianTasks");

            migrationBuilder.DropColumn(
                name: "IdResponsible",
                table: "GuardianTasks");

            migrationBuilder.UpdateData(
                table: "GuardianTasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 1, 26, 21, 46, 33, 307, DateTimeKind.Local).AddTicks(5215));
        }
    }
}
