using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials37 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "Zagreb" });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Velika Gorica" });

            migrationBuilder.InsertData(
                table: "City",
                columns: new[] { "ID", "Name" },
                values: new object[] { 3, "Vrbovsko" });
        }
    }
}
