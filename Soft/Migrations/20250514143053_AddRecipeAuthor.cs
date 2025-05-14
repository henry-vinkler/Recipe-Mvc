using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMvc.Soft.Migrations
{
    /// <inheritdoc />
    public partial class AddRecipeAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserAccountDataId",
                table: "Recipes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserAccountDataId",
                table: "Recipes",
                column: "UserAccountDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_UserAccounts_UserAccountDataId",
                table: "Recipes",
                column: "UserAccountDataId",
                principalTable: "UserAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_UserAccounts_UserAccountDataId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_UserAccountDataId",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "UserAccountDataId",
                table: "Recipes");
        }
    }
}
