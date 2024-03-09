using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials24 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Manufacturers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Manufacturers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Manufacturers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Manufacturers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Manufacturers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Manufacturers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Manufacturers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Manufacturers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Manufacturers");
        }
    }
}
