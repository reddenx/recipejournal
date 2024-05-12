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
        bool UpdateShoppingList(Guid userId, ShoppingRecipeScale[] recipeIds, Guid[] gatheredIngredientIds, NonrecipeIngredient[] nonrecipeIngredients);
    }
    public class NonrecipeIngredient
    {
        public Guid IngredientId { get; set; }
        public string Amount { get; set; }
    }
    public class NonrecipeShoppingListIngredient
    {
        public Guid IngredientId { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
    }
    public class ShoppingList
    {
        public ShoppingRecipe[] Recipes { get; set; }
        public GatheredIngredient[] GatheredIngredients { get; set; }
        public NonrecipeShoppingListIngredient[] NonrecipeIngredients { get; set; }
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
                inner join recipe r on r.Id = sr.RecipeId
                inner join step_ingredient si on si.RecipeId = sr.RecipeId
                inner join ingredient i on i.Id = si.IngredientId
            where sr.UserId = @Id";

            var sqlGathered = @"
            select
                g.UserId,
                g.IngredientId,
                i.Name
            from shopping_gathered g
                inner join ingredient i on i.Id = g.IngredientId
            where g.UserId = @Id";

            var sqlNonrecipe = @"
            select
                nr.UserId,
                nr.IngredientId,
                i.Name,
                nr.Amount
            from shopping_nonrecipe_ingredient nr
                inner join ingredient i on i.Id = nr.IngredientId
            where nr.UserId = @Id";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var recipes = conn.Query<ShoppingRecipeData>(sqlRecipes, new { Id = userId.ToString("N") });
                var gathered = conn.Query<ShoppingGatheredData>(sqlGathered, new { Id = userId.ToString("N") });
                var nonrecipe = conn.Query<ShoppingNonrecipeData>(sqlNonrecipe, new { Id = userId.ToString("N") });

                return new ShoppingList
                {
                    GatheredIngredients = gathered.Select(i => new GatheredIngredient
                    {
                        Id = Guid.Parse(i.IngredientId),
                        Name = i.Name,
                    }).ToArray(),
                    Recipes = recipes.GroupBy(r => r.RecipeId).Select(r => new ShoppingRecipe
                    {
                        Id = Guid.Parse(r.Key),
                        Scale = r.First().Scale,
                        Title = r.First().Title,
                        Ingredients = r.Select(i => new ShoppingRecipeIngredient
                        {
                            Id = Guid.Parse(i.IngredientId),
                            Amount = i.Amount,
                            Name = i.Name,
                            Unit = i.Unit
                        }).ToArray(),
                    }).ToArray(),
                    NonrecipeIngredients = nonrecipe.Select(nr => new NonrecipeShoppingListIngredient
                    {
                        IngredientId = Guid.Parse(nr.IngredientId),
                        Name = nr.Name,
                        Amount = nr.Amount
                    }).ToArray()
                };
            }
        }

        public bool UpdateShoppingList(Guid userId, ShoppingRecipeScale[] recipeScales, Guid[] gatheredIngredientIds, NonrecipeIngredient[] nonrecipeIngredients)
        {
            var delRecipes = @"
            delete from shopping_recipe
            where UserId = @Id";
            var delGathered = @"
            delete from shopping_gathered
            where UserId = @Id";
            var delNonrecipe = @"
            delete from shopping_nonrecipe_ingredient
            where UserId = @Id";

            //ugh I wish mysql had tvp and merge
            var insertRecipe = @"
            insert into shopping_recipe (RecipeId, UserId, Scale)
            values (@RecipeId, @UserId, @Scale)";
            var insertGathered = @"
            insert into shopping_gathered (UserId, IngredientId)
            values (@UserId, @IngredientId)";
            var insertNonrecipe = @"
            insert into shopping_nonrecipe_ingredient
            values (@UserId, @IngredientId, @Amount)";

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Execute(delRecipes, new { Id = userId.ToString("N") });
                conn.Execute(delGathered, new { Id = userId.ToString("N") });
                conn.Execute(delNonrecipe, new { Id = userId.ToString("N") });

                foreach (var scale in recipeScales)
                {
                    conn.Execute(insertRecipe, new
                    {
                        RecipeId = scale.Id.ToString("N"),
                        UserId = userId.ToString("N"),
                        Scale = scale.Scale
                    });
                }
                foreach (var ingredientId in gatheredIngredientIds)
                {
                    conn.Execute(insertGathered, new
                    {
                        UserId = userId.ToString("N"),
                        IngredientId = ingredientId.ToString("N")
                    });
                }
                foreach (var nonRecIngredient in nonrecipeIngredients)
                {
                    conn.Execute(insertNonrecipe, new
                    {
                        UserId = userId.ToString("N"),
                        IngredientId = nonRecIngredient.IngredientId.ToString("N"),
                        Amount = nonRecIngredient.Amount
                    });
                }
            }
            return true;
        }

        public class ShoppingRecipeData
        {
            public string RecipeId { get; set; }
            public string UserId { get; set; }
            public float Scale { get; set; }
            public string IngredientId { get; set; }
            public float Amount { get; set; }
            public string Unit { get; set; }
            public string Title { get; set; }
            public string Name { get; set; }
        }
        public class ShoppingGatheredData
        {
            public string UserId { get; set; }
            public string IngredientId { get; set; }
            public string Name { get; set; }
        }
        public class ShoppingNonrecipeData
        {
            public string UserId { get; set; }
            public string IngredientId { get; set; }
            public string Name { get; set; }
            public string Amount { get; set; }
        }
    }

    public class MockShoppingRepository : IShoppingRepository
    {
        private static readonly Dictionary<Guid, ShoppingList> _shoppingLists = new Dictionary<Guid, ShoppingList>();

        public ShoppingList GetUserShoppingList(Guid userId)
        {
            if (!_shoppingLists.ContainsKey(userId))
            {
                var repo = new MockRecipeRepository();
                var special = repo.GetRecipe(Guid.Empty).Components.Single().Steps.Single().Ingredients
                    .Select(i => new NonrecipeShoppingListIngredient
                    {
                        IngredientId = i.Id,
                        Name = i.Name,
                        Amount = "test amount"
                    });

                _shoppingLists.Add(userId, new ShoppingList
                {
                    GatheredIngredients = new GatheredIngredient[] { },
                    Recipes = new ShoppingRecipe[] { },
                    NonrecipeIngredients = special.ToArray()
                });
            }

            return _shoppingLists[userId];
        }

        public bool UpdateShoppingList(Guid userId, ShoppingRecipeScale[] recipeIds, Guid[] gatheredIngredientIds, NonrecipeIngredient[] nonrecipeIngredients)
        {
            var repo = new MockRecipeRepository();

            if (!_shoppingLists.ContainsKey(userId))
                _shoppingLists.Add(userId, new ShoppingList
                {
                    GatheredIngredients = new GatheredIngredient[] { },
                    Recipes = new ShoppingRecipe[] { },
                    NonrecipeIngredients = new NonrecipeShoppingListIngredient[] { }
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

            var allPossibleIngredients = recipes.SelectMany(r => r.Ingredients)
                .Select(i => new { i.Id, i.Name })
                .Concat(repo.GetRecipe(Guid.Empty).Components[0].Steps[0].Ingredients.Select(i => new { i.Id, i.Name }))
                .ToArray();

            var ingredients = gatheredIngredientIds
                .Where(id => allPossibleIngredients.Any(i => i.Id == id))
                .Select(id => new GatheredIngredient
                {
                    Id = id,
                    Name = allPossibleIngredients.First(i => i.Id == id).Name,
                }).ToArray();

            var nrIngredients = nonrecipeIngredients.Select(nr => new NonrecipeShoppingListIngredient
            {
                Amount = nr.Amount,
                IngredientId = nr.IngredientId,
                Name = allPossibleIngredients.FirstOrDefault(i => i.Id == nr.IngredientId)?.Name ?? "unknown"
            }).ToArray();

            var list = _shoppingLists[userId];
            list.Recipes = recipes;
            list.GatheredIngredients = ingredients;
            list.NonrecipeIngredients = nrIngredients;

            return true;
        }
    }
}