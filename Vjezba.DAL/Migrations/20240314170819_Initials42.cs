using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Pristigla" });

            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Na čekanju" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "ID",
                keyValue: 2);
        }
    }
}
