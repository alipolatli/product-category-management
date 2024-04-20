using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace product_category.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_ParentId",
                table: "categories");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "categories",
                newName: "parentId");

            migrationBuilder.RenameIndex(
                name: "IX_categories_ParentId",
                table: "categories",
                newName: "IX_categories_parentId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_parentId",
                table: "categories",
                column: "parentId",
                principalTable: "categories",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_categories_parentId",
                table: "categories");

            migrationBuilder.RenameColumn(
                name: "parentId",
                table: "categories",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_categories_parentId",
                table: "categories",
                newName: "IX_categories_ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_categories_ParentId",
                table: "categories",
                column: "ParentId",
                principalTable: "categories",
                principalColumn: "id");
        }
    }
}
