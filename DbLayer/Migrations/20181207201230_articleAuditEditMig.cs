using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Migrations
{
    public partial class articleAuditEditMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "ReadedArticles");

            migrationBuilder.AddColumn<int>(
                name: "ReadCount",
                table: "ReadedArticles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Article",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadCount",
                table: "ReadedArticles");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "ReadedArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ImagePath",
                table: "Article",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
