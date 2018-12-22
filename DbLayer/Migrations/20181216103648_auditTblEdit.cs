using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBaseContext.Migrations
{
    public partial class auditTblEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Author_AuthorId",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_AuthorId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "ReadedArticles");

            migrationBuilder.RenameColumn(
                name: "IsLike",
                table: "ReadedArticles",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Article",
                newName: "ModifiedBy");

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "ReadedArticles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedBy",
                table: "Author",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_CreatedBy",
                table: "Article",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Author_CreatedBy",
                table: "Article",
                column: "CreatedBy",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Author_CreatedBy",
                table: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Article_CreatedBy",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ReadedArticles");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Author");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "ReadedArticles",
                newName: "IsLike");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Article",
                newName: "AuthorId");

            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "ReadedArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Article_AuthorId",
                table: "Article",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Author_AuthorId",
                table: "Article",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
