using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static RecipeJournalApi.Controllers.RecipeController;

namespace RecipeJournalApi.Controllers
{
    public interface IRecipeRepository
    {
        Recipe GetRecipe(Guid id);
        RecipeListItem[] GetRecipesForUser(Guid userId);
        Recipe UpdateRecipe(RecipeDto update);
    }
    public class RecipeListItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int? DurationMinutes { get; set; }
        public int? Servings { get; set; }
    }
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public Guid AuthorId { get; set; }
        public int Version { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime VersionDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? DurationMinutes { get; set; }
        public int? Servings { get; set; }
        public bool IsDraft { get; set; }
        public bool IsPublic { get; set; }

        public RecipeComponent[] Components { get; set; }

        public class RecipeComponent
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public RecipeStep[] Steps { get; set; }
        }

        public class RecipeStep
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }

            public RecipeIngredient[] Ingredients { get; set; }
        }

        public class RecipeIngredient
        {
            public Guid Id { get; set; }
            public Guid IngredientId { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Description { get; set; }
            public float Amount { get; set; }
        }
    }

    public class MockRecipeRepository : IRecipeRepository
    {
        public Recipe GetRecipe(Guid id)
        {
            return _recipeDB.FirstOrDefault(r => r.Id == id);
        }

        public RecipeListItem[] GetRecipesForUser(Guid userId)
        {
            return _recipeDB.Where(r => r.AuthorId == userId).Select(r => new RecipeListItem
            {
                Id = r.Id,
                Title = r.Title
            }).ToArray();
        }

        public Recipe UpdateRecipe(RecipeDto update)
        {
            throw new NotImplementedException();
        }

        private static readonly List<Recipe> _recipeDB = new List<Recipe>()
        {
            new Recipe()
            {
                AuthorId = MockUserRepository.MOCK_USER,
                Author = "sean",
                DateCreated = DateTime.Now,
                VersionDate = DateTime.Now,
                Description = "German Apply Pancakes",
                Title = "fuffy cakes w/ apples",
                Id = Guid.NewGuid(),
                DurationMinutes = 30,
                Servings = 2,
                Version = 1,
                Components = new []
                {
                    new Recipe.RecipeComponent
                    {
                        Id = Guid.NewGuid(),
                        Title = "",
                        Description = "",
                        Steps = new []
                        {
                            new Recipe.RecipeStep
                            {
                                Id = Guid.NewGuid(),
                                Title = "step 1",
                                Body = "mix together eggs, flour, salt, milk.",
                                Ingredients = new []
                                {
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Name = "egg",
                                        Description = "",
                                        Amount = 4,
                                        Unit = "count",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Name = "flour",
                                        Description = "",
                                        Amount = .75f,
                                        Unit = "cup",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Name = "salt",
                                        Description = "",
                                        Amount = 1,
                                        Unit = "teaspoon",
                                    },
                                }
                            },
                            new Recipe.RecipeStep
                            {
                                Id = Guid.NewGuid(),
                                Title = "step 2",
                                Body = "melt butter into pans, place cut apples into pans, concentric circles usually work best. I need some longer content here to test wrapping and display ugh",
                                Ingredients = new []
                                {
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Name = "butter",
                                        Description = "",
                                        Amount = 4,
                                        Unit = "tablespoon",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Name = "apple",
                                        Description = "",
                                        Amount = 2,
                                        Unit = "count",
                                    },
                                }
                            },
                            new Recipe.RecipeStep
                            {
                                Id = Guid.NewGuid(),
                                Title = "step 3",
                                Body = "mix together sugar and cinnamon",
                                Ingredients = new []
                                {
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Name = "sugar",
                                        Description = "",
                                        Amount = .33f,
                                        Unit = "cup",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Name = "cinnamon",
                                        Description = "",
                                        Amount = 3,
                                        Unit = "teaspoon",
                                    },
                                }
                            },
                            new Recipe.RecipeStep
                            {
                                Id = Guid.NewGuid(),
                                Title = "step 4",
                                Body = "pour mixture over apples",
                                Ingredients = new Recipe.RecipeIngredient[]{}
                            },
                            new Recipe.RecipeStep
                            {
                                Id = Guid.NewGuid(),
                                Title = "step 5",
                                Body = "sprinkle surgar over pans, covering them evenly",
                                Ingredients = new Recipe.RecipeIngredient[]{}
                            },
                        }
                    }
                }
            }
        };
    }
}