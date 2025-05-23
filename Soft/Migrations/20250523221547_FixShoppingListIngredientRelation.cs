using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeMvc.Soft.Migrations
{
    /// <inheritdoc />
    public partial class FixShoppingListIngredientRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShoppingListDataId",
                table: "ShoppingListIngredients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListIngredients_ShoppingListDataId",
                table: "ShoppingListIngredients",
                column: "ShoppingListDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListIngredients_ShoppingLists_ShoppingListDataId",
                table: "ShoppingListIngredients",
                column: "ShoppingListDataId",
                principalTable: "ShoppingLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListIngredients_ShoppingLists_ShoppingListDataId",
                table: "ShoppingListIngredients");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListIngredients_ShoppingListDataId",
                table: "ShoppingListIngredients");

            migrationBuilder.DropColumn(
                name: "ShoppingListDataId",
                table: "ShoppingListIngredients");
        }
    }
}
