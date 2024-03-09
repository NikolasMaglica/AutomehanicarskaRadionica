using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vjezba.DAL.Migrations
{
    public partial class Initials26 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Materials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Materials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Materials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Materials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Materials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteTime",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedById",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Clients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Clients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DeleteTime",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Clients");
        }
    }
}
