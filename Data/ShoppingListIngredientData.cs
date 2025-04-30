using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeMvc.Data
{
    public sealed class ShoppingListIngredientData:EntityData<ShoppingListIngredientData>
    {
        public int ShoppingListID { get; set; }
        public int IngredientID { get; set; }
        public string Quantity { get; set; } = string.Empty;
        public bool IsChecked { get; set; } = false;

    }
}
