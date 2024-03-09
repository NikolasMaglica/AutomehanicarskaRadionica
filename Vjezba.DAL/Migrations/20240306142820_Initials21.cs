using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_AppUsersId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "AppUsersId",
                table: "Offers",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_AppUsersId",
                table: "Offers",
                newName: "IX_Offers_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_UsersId",
                table: "Offers",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_AspNetUsers_UsersId",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "Offers",
                newName: "AppUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_UsersId",
                table: "Offers",
                newName: "IX_Offers_AppUsersId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_AspNetUsers_AppUsersId",
                table: "Offers",
                column: "AppUsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
