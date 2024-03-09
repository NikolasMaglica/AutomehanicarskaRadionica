using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Offers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Offers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Offers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Offers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Offers");
        }
    }
}
