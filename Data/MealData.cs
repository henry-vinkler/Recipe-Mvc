using System;

namespace RecipeMvc.Data;

public sealed class MealData: EntityData<MealData>
{
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
}
