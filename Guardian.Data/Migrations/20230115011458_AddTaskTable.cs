using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guardian.Data.Migrations
{
    public partial class AddTaskTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "GuardianTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaksName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuardianTasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GuardianTasks",
                columns: new[] { "Id", "Category", "CreatedDate", "Description", "Priority", "Status", "TaksName", "UpdatedDate" },
                values: new object[] { 1, 1, new DateTime(2023, 1, 14, 22, 14, 58, 499, DateTimeKind.Local).AddTicks(9), "Testing fist API by myself", 1, true, "First Task", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GuardianTasks");

            migrationBuilder.InsertData(
                table: "Guardians",
                columns: new[] { "Id", "Adress", "Age", "CreatedDate", "DeletedeDate", "Name", "Occupancy", "Status", "UpdatedDate" },
                values: new object[] { 1, "São Paulo", 22, new DateTime(2023, 1, 4, 21, 45, 17, 187, DateTimeKind.Local).AddTicks(4120), null, "Felippe Delesporte", "Software Developer", true, null });
        }
    }
}
