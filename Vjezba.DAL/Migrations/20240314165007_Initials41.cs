using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials41 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OfferStatuses",
                columns: new[] { "ID", "OfferStatusName" },
                values: new object[] { 1, "Prihvaćena" });

            migrationBuilder.InsertData(
                table: "OfferStatuses",
                columns: new[] { "ID", "OfferStatusName" },
                values: new object[] { 2, "Odbijena" });

            migrationBuilder.InsertData(
                table: "OfferStatuses",
                columns: new[] { "ID", "OfferStatusName" },
                values: new object[] { 3, "Na čekanju" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OfferStatuses",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OfferStatuses",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OfferStatuses",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
