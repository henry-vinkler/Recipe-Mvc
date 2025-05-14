using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeMvc.Data;

public sealed class ShoppingListData : EntityData<ShoppingListData>
{
    public int UserID { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsChecked { get; set; } = false;
    public string Notes { get; set; } = string.Empty;

}


