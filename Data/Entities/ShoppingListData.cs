namespace RecipeMvc.Data.Entities
{
    public sealed class ShoppingListData:EntityData<ShoppingListData>
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;
        public string Notes { get; set; } = string.Empty;
        public ICollection<ShoppingListIngredientData> Ingredients { get; set; } = new List<ShoppingListIngredientData>();
    }
}
