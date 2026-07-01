using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipePlanner.Data.Migrations
{
    /// <inheritdoc />
    public partial class RecipeTagEndpoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByUserID",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedByUserID",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedByUserID",
                table: "Tags",
                column: "CreatedByUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ModifiedByUserID",
                table: "Tags",
                column: "ModifiedByUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Users_CreatedByUserID",
                table: "Tags",
                column: "CreatedByUserID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Users_ModifiedByUserID",
                table: "Tags",
                column: "ModifiedByUserID",
                principalTable: "Users",
                principalColumn: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Users_CreatedByUserID",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Users_ModifiedByUserID",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_CreatedByUserID",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ModifiedByUserID",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CreatedByUserID",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ModifiedByUserID",
                table: "Tags");
        }
    }
}
