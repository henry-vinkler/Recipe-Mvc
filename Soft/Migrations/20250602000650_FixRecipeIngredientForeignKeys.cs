using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMvc.Soft.Migrations
{
    /// <inheritdoc />
    public partial class FixRecipeIngredientForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_Recipes_RecipeId",
                table: "Favourites");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropIndex(
                name: "IX_Favourites_RecipeId",
                table: "Favourites");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "ShoppingLists",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ShoppingListID",
                table: "ShoppingListIngredients",
                newName: "ShoppingListId");

            migrationBuilder.RenameColumn(
                name: "IngredientID",
                table: "ShoppingListIngredients",
                newName: "IngredientId");

            migrationBuilder.AddColumn<int>(
                name: "RecipeDataId",
                table: "RecipeIngredients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListIngredients_IngredientId",
                table: "ShoppingListIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredients_RecipeDataId",
                table: "RecipeIngredients",
                column: "RecipeDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeDataId",
                table: "RecipeIngredients",
                column: "RecipeDataId",
                principalTable: "Recipes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListIngredients_Ingredients_IngredientId",
                table: "ShoppingListIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredients_Recipes_RecipeDataId",
                table: "RecipeIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListIngredients_Ingredients_IngredientId",
                table: "ShoppingListIngredients");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListIngredients_IngredientId",
                table: "ShoppingListIngredients");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredients_RecipeDataId",
                table: "RecipeIngredients");

            migrationBuilder.DropColumn(
                name: "RecipeDataId",
                table: "RecipeIngredients");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ShoppingLists",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "ShoppingListId",
                table: "ShoppingListIngredients",
                newName: "ShoppingListID");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "ShoppingListIngredients",
                newName: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_RecipeId",
                table: "Favourites",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_Recipes_RecipeId",
                table: "Favourites",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredients_Ingredients_IngredientId",
                table: "RecipeIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
