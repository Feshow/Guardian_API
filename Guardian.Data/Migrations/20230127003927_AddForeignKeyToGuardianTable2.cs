using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guardian.Data.Migrations
{
    public partial class AddForeignKeyToGuardianTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GuardianTasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 1, 26, 21, 39, 26, 884, DateTimeKind.Local).AddTicks(6522));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GuardianTasks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 1, 26, 21, 34, 44, 546, DateTimeKind.Local).AddTicks(9549));
        }
    }
}
