using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixRecipeIngredientMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeDataId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_RecipeDataId",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "RecipeDataId",
                table: "RecipeIngredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecipeDataId",
                table: "RecipeIngredients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeDataId",
                table: "RecipeIngredients",
                column: "RecipeDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeDataId",
                table: "RecipeIngredients",
                column: "RecipeDataId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
