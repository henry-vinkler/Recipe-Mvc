﻿using RecipeMvc.Data.Entities;
namespace RecipeMvc.Facade.Favorite;
public sealed class FavouriteViewFactory : AbstractViewFactory<FavouriteData, FavouriteView> {
    public FavouriteView Create(FavouriteData d) => new FavouriteView {
       UserId = d.UserId,
       RecipeId = d.RecipeId,
    };

    public IList<FavouriteView> CreateMany(IEnumerable<FavouriteData> items)
        => items.Select(Create).ToList();

}