using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace product_category.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryAttributeValues_category-attributes_categoryAttribu~",
                table: "CategoryAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryAttributeValues",
                table: "CategoryAttributeValues");

            migrationBuilder.RenameTable(
                name: "CategoryAttributeValues",
                newName: "category-attribute-values");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryAttributeValues_categoryAttributeId",
                table: "category-attribute-values",
                newName: "IX_category-attribute-values_categoryAttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category-attribute-values",
                table: "category-attribute-values",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_category-attribute-values_category-attributes_categoryAttri~",
                table: "category-attribute-values",
                column: "categoryAttributeId",
                principalTable: "category-attributes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category-attribute-values_category-attributes_categoryAttri~",
                table: "category-attribute-values");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category-attribute-values",
                table: "category-attribute-values");

            migrationBuilder.RenameTable(
                name: "category-attribute-values",
                newName: "CategoryAttributeValues");

            migrationBuilder.RenameIndex(
                name: "IX_category-attribute-values_categoryAttributeId",
                table: "CategoryAttributeValues",
                newName: "IX_CategoryAttributeValues_categoryAttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryAttributeValues",
                table: "CategoryAttributeValues",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryAttributeValues_category-attributes_categoryAttribu~",
                table: "CategoryAttributeValues",
                column: "categoryAttributeId",
                principalTable: "category-attributes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
