using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIngredientRelationToRecipeIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientId1",
                table: "RecipeIngredients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_IngredientId1",
                table: "RecipeIngredients",
                column: "IngredientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId1",
                table: "RecipeIngredients",
                column: "IngredientId1",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId1",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_IngredientId1",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "IngredientId1",
                table: "RecipeIngredients");
        }
    }
}
