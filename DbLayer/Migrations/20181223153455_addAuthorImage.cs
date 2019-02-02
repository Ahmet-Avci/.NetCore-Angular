using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Migrations
{
    public partial class addAuthorImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Autobiography",
                table: "Author",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Author",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Autobiography",
                table: "Author");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Author");
        }
    }
}
