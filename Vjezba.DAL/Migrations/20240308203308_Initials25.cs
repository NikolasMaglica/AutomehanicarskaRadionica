using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Services",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Services",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Services",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Services",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Services");
        }
    }
}
