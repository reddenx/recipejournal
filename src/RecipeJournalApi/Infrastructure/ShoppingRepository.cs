using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

namespace RecipeJournalApi.Infrastructure
{
    public interface IShoppingRepository
    {
        ShoppingList GetUserShoppingList(Guid userId);
        bool UpdateShoppingList(Guid userId, ShoppingRecipeScale[] recipeIds, Guid[] gatheredIngredientIds);
    }
    public class ShoppingList
    {
        public ShoppingRecipe[] Recipes { get; set; }
        public GatheredIngredient[] GatheredIngredients { get; set; }
    }
    public class ShoppingRecipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public float Scale { get; set; }
        public ShoppingRecipeIngredient[] Ingredients { get; set; }
    }
    public class ShoppingRecipeIngredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float Amount { get; set; }
    }
    public class GatheredIngredient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class ShoppingRecipeScale
    {
        public Guid Id { get; set; }
        public float Scale { get; set; }
    }

    public class MockShoppingRepository : IShoppingRepository
    {
        private static readonly Dictionary<Guid, ShoppingList> _shoppingLists = new Dictionary<Guid, ShoppingList>();

        public ShoppingList GetUserShoppingList(Guid userId)
        {
            if (!_shoppingLists.ContainsKey(userId))
                _shoppingLists.Add(userId, new ShoppingList
                {
                    GatheredIngredients = new GatheredIngredient[] { },
                    Recipes = new ShoppingRecipe[] { }
                });

            return _shoppingLists[userId];
        }

        public bool UpdateShoppingList(Guid userId, ShoppingRecipeScale[] recipeIds, Guid[] gatheredIngredientIds)
        {
            var repo = new MockRecipeRepository();

            if (!_shoppingLists.ContainsKey(userId))
                _shoppingLists.Add(userId, new ShoppingList
                {
                    GatheredIngredients = new GatheredIngredient[] { },
                    Recipes = new ShoppingRecipe[] { }
                });

            var recipes = recipeIds.Select(r =>
            {
                var recipe = repo.GetRecipe(r.Id);
                return new ShoppingRecipe
                {
                    Id = r.Id,
                    Scale = r.Scale,
                    Title = recipe.Title,
                    Ingredients = recipe.Components
                        .SelectMany(c => c.Steps)
                        .SelectMany(s => s.Ingredients)
                        .Select(i => new ShoppingRecipeIngredient
                        {
                            Amount = i.Amount,
                            Id = i.Id,
                            Name = i.Name,
                            Unit = i.Unit
                        }).ToArray()
                };
            }).ToArray();

            var ingredients = recipes.SelectMany(r => r.Ingredients)
                .Where(i => gatheredIngredientIds.Contains(i.Id))
                .Select(i => new GatheredIngredient
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToArray();

            var list = _shoppingLists[userId];
            list.Recipes = recipes;
            list.GatheredIngredients = ingredients;

            return true;
        }
    }
}