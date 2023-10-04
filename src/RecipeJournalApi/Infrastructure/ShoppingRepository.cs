using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Misc;

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

    public class ShoppingRepository : IShoppingRepository
    {
        private readonly string _connectionString;

        public ShoppingRepository(IDbConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public ShoppingList GetUserShoppingList(Guid userId)
        {
            var sqlRecipes = @"
            select 
                sr.RecipeId,
                sr.UserId,
                sr.Scale,
                si.IngredientId,
                si.Amount,
                si.Unit,
                r.Title,
                i.Name
            from shopping_recipe sr
                inner join recipe r on r.Id = st.RecipeId
                inner join step_ingredient si on si.RecipeId = r.RecipeId
                inner join ingredient i on i.Id = si.IngredientId
            where r.UserId = @Id";

            var sqlGathered = @"
            select
                g.UserId,
                g.IngredientId,
                i.Name
            from shopping_gathered g
                inner join ingredient i on i.Id = g.IngredientId
            where i.UserId = @Id";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var recipes = conn.Query<ShoppingRecipeData>(sqlRecipes, new { Id = userId.ToString("N") });
                var gathered = conn.Query<ShoppingGatheredData>(sqlGathered, new { Id = userId.ToString("N") });

                return new ShoppingList
                {
                    GatheredIngredients = gathered.Select(i => new GatheredIngredient
                    {
                        Id = i.IngredientId,
                        Name = i.Name,
                    }).ToArray(),
                    Recipes = recipes.GroupBy(r => r.RecipeId).Select(r => new ShoppingRecipe
                    {
                        Id = r.Key,
                        Scale = r.First().Scale,
                        Title = r.First().Title,
                        Ingredients = r.Select(i => new ShoppingRecipeIngredient
                        {
                            Id = i.IngredientId,
                            Amount = i.Amount,
                            Name = i.Name,
                            Unit = i.Unit
                        }).ToArray(),
                    }).ToArray(),
                };
            }
        }

        public bool UpdateShoppingList(Guid userId, ShoppingRecipeScale[] recipeScales, Guid[] gatheredIngredientIds)
        {
            var delRecipes = @"
            delete from shopping_recipe
            where UserId = @Id";
            var delGathered = @"
            delete from shopping_gathered
            where UserId = @Id";

            //ugh I wish mysql had tvp and merge
            var insertRecipe = @"
            insert into shopping_recipe (RecipeId, UserId, Scale)
            values (@RecipeId, @UserId, @Scale)";
            var insertGathered = @"
            insert into shopping_gathered (UserId, IngredientId)
            values (@UserId, @IngredientId)";

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Execute(delRecipes, new { Id = userId.ToString("N") });
                conn.Execute(delGathered, new { Id = userId.ToString("N") });

                foreach(var scale in recipeScales)
                {
                    conn.Execute(insertRecipe, new 
                    {
                        RecipeId = scale.Id.ToString("N"),
                        UserId = userId.ToString("N"),
                        Scale = scale.Scale
                    });
                }
                foreach(var ingredientId in gatheredIngredientIds)
                {
                    conn.Execute(insertGathered, new 
                    {
                        UserId = userId.ToString("N"),
                        IngredientId = ingredientId.ToString("N")
                    });
                }
            }
            return true;
        }

        public class ShoppingRecipeData
        {
            public Guid RecipeId { get; set; }
            public Guid UserId { get; set; }
            public float Scale { get; set; }
            public Guid IngredientId { get; set; }
            public float Amount { get; set; }
            public string Unit { get; set; }
            public string Title { get; set; }
            public string Name { get; set; }
        }
        public class ShoppingGatheredData
        {
            public Guid UserId { get; set; }
            public Guid IngredientId { get; set; }
            public string Name { get; set; }
        }

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