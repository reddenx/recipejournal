using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySql.Data.MySqlClient;
using static RecipeJournalApi.Controllers.RecipeController;

namespace RecipeJournalApi.Infrastructure
{
    public interface IRecipeRepository
    {
        Recipe GetRecipe(Guid id);
        RecipeListItem[] GetRecipesForUser(Guid? userId);
        Recipe UpdateRecipe(RecipeInputDto update, UserInfo user);
    }
    public class RecipeListItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int? DurationMinutes { get; set; }
        public int? Servings { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
        public int Version { get; set; }
        public float Rating { get; set; }
        public string[] Tags { get; set; }
        public int TotalJournalCount { get; set; }
        public bool IsPublic { get; set; }
        public bool IsDraft { get; set; }
        public LoggedInUserRecipeListItemInfo LoggedInUserInfo { get; set; }

        public class LoggedInUserRecipeListItemInfo
        {
            public float? PersonalBest { get; set; }
            public int JournalCount { get; set; }
            public DateTime? LatestJournalEntryDate { get; set; }
            public int GoalCount { get; set; }
            public int NoteCount { get; set; }
        }
    }
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Guid AuthorId { get; set; }
        public int? DurationMinutes { get; set; }
        public int? Servings { get; set; }
        public int Version { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime VersionDate { get; set; }
        public float Rating { get; set; }
        public string Description { get; set; }
        public bool IsDraft { get; set; }
        public bool IsPublic { get; set; }
        public string[] Tags { get; set; }

        public RecipeComponent[] Components { get; set; }

        public class RecipeComponent
        {
            public Guid Id { get; set; }
            public int Number { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }

            public RecipeStep[] Steps { get; set; }
        }

        public class RecipeStep
        {
            public Guid Id { get; set; }
            public int Number { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }

            public RecipeIngredient[] Ingredients { get; set; }
        }

        public class RecipeIngredient
        {
            public Guid Id { get; set; }
            public Guid IngredientId { get; set; }
            public int Number { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Description { get; set; }
            public float Amount { get; set; }
        }
    }

    public interface IDbConfig
    {
        string ConnectionString { get; }
    }
    public class RecipeRepository : IRecipeRepository
    {
        private readonly string _connectionString;

        public RecipeRepository(IDbConfig config)
        {
            _connectionString = config.ConnectionString;
        }

        public Recipe GetRecipe(Guid id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                return GetRecipeWithConnection(conn, id);
            }
        }

        private Recipe GetRecipeWithConnection(MySqlConnection conn, Guid id)
        {
            var sqlRecipe = @"
            select 
                r.Id, 
                r.AccountId, 
                a.Username, 
                r.Version, 
                r.DateCreated, 
                r.VersionDate,
                r.Title,
                r.Description,
                r.DurationMinutes,
                r.Servings,
                r.Published,
                r.Public
            from recipe r
                inner join account a on a.Id = r.AccountId
            where r.Id = @Id";
            var sqlComponent = @"
            select
                c.Id,
                c.Number,
                c.RecipeId,
                c.Title,
                c.Description
            from recipe_component c
            where c.RecipeId = @Id";
            var sqlStep = @"
            select
                s.Id,
                s.Number,
                s.RecipeId,
                s.ComponentId,
                s.Title,
                s.Body
            from recipe_step s
            where s.RecipeId = @Id";
            var sqlIngredients = @"
            select
                s.Id,
                s.StepId,
                s.RecipeId,
                s.IngredientId,
                s.Number,
                i.Name,
                s.Description,
                s.Unit,
                s.Amount
            from step_ingredient s
                inner join ingredient i on i.Id = s.IngredientId
            where s.RecipeId = @Id";
            var recipe = conn.Query<RecipeData>(sqlRecipe, new { Id = id.ToString("N") }).FirstOrDefault();

            if (recipe == null)
                return null;

            var components = conn.Query<RecipeComponentData>(sqlComponent, new { Id = id.ToString("N") });
            var steps = conn.Query<RecipeStepData>(sqlStep, new { Id = id.ToString("N") });
            var ingredients = conn.Query<RecipeIngredientData>(sqlIngredients, new { Id = id.ToString("N") });

            var result = FromData(recipe, components.ToArray(), steps.ToArray(), ingredients.ToArray());
            return result;
        }

        public RecipeListItem[] GetRecipesForUser(Guid? userId)
        {
            var sqlRecipe = @"
            select 
                r.Id, 
                r.AccountId, 
                a.Username, 
                r.Version, 
                r.DateCreated, 
                r.VersionDate,
                r.Title,
                r.Description,
                r.DurationMinutes,
                r.Servings,
                r.Published,
                r.Public
            from recipe r
                inner join account a on a.Id = r.AccountId
            where r.Public = 1 or r.AccountId = @Id";
            var sqlJournal = @"
            select
                j.Id,
                j.RecipeId,
                j.UserId,
                j.StickyNext,
                j.EntryDate,
                j.NextDismissed,
                j.SuccessRating
            from recipe_journal_entry j
            where j.UserId = @Id";

            using (var conn = new MySqlConnection(_connectionString))
            {
                var recipes = conn.Query<RecipeData>(sqlRecipe, new { Id = userId?.ToString("N") }).ToArray();
                var journalResult = conn.Query<JournalListData>(sqlJournal, new { Id = userId?.ToString("N") });

                return recipes.Select(r =>
                {
                    var journal = journalResult.Where(j => j.RecipeId == r.Id);
                    return new RecipeListItem
                    {
                        Id = Guid.Parse(r.Id),
                        DurationMinutes = r.DurationMinutes,
                        Servings = r.Servings,
                        Title = r.Title,
                        Author = r.Username,
                        DateCreated = r.DateCreated,
                        IsDraft = !r.Published,
                        IsPublic = r.Public,
                        LastModified = r.VersionDate,
                        Rating = 0,
                        Tags = new string[] { },
                        TotalJournalCount = 0,
                        Version = r.Version,
                        LoggedInUserInfo = userId.HasValue ? new RecipeListItem.LoggedInUserRecipeListItemInfo
                        {
                            GoalCount = 0,
                            JournalCount = journal.Count(),
                            LatestJournalEntryDate = journal.MaxBy(j => j.EntryDate)?.EntryDate,
                            NoteCount = journal.Count(j => j.StickyNext && !j.NextDismissed),
                            PersonalBest = journal.MaxBy(j => j.SuccessRating)?.SuccessRating
                        } : null,
                    };
                }).ToArray();
            }
        }
        class JournalListData
        {
            public string Id { get; set; }
            public string RecipeId { get; set; }
            public string UserId { get; set; }
            public bool StickyNext { get; set; }
            public DateTime EntryDate { get; set; }
            public bool NextDismissed { get; set; }
            public float SuccessRating { get; set; }
        }

        public Recipe UpdateRecipe(RecipeInputDto update, UserInfo user)
        {
            Guid? recipeId = update.Id;
            using (var conn = new MySqlConnection(_connectionString))
            {
                if (update.Id.HasValue)
                {
                    var current = GetRecipeWithConnection(conn, update.Id.Value);
                    var newVersion = current.Version + 1;

                    const string sqlDelComponents = @"
                    delete from recipe_component
                    where RecipeId = @Id";

                    conn.Execute(sqlDelComponents, new { Id = current.Id.ToString("N") });

                    const string sqlDelSteps = @"
                    delete from recipe_step
                    where RecipeId = @Id";

                    conn.Execute(sqlDelSteps, new { Id = current.Id.ToString("N") });

                    const string sqlDelIngredients = @"
                    delete from step_ingredient
                    where RecipeId = @Id";

                    conn.Execute(sqlDelIngredients, new { Id = current.Id.ToString("N") });

                    //TODO write version select so multiple versions can exist in the db
                    const string sqlUpdateRecipe = @"
                    update recipe
                    set
                        Version = @Version,
                        VersionDate = @VersionDate,
                        Title = @Title,
                        Description = @Description,
                        DurationMinutes = @DurationMinutes,
                        Servings = @Servings,
                        Published = @Published,
                        Public = @Public
                    where Id = @Id";

                    conn.Execute(sqlUpdateRecipe, new
                    {
                        Version = newVersion,
                        VersionDate = DateTime.Now,
                        Title = update.Title,
                        Description = update.Description,
                        DurationMinutes = update.DurationMinutes,
                        Servings = update.Servings,
                        Published = !update.IsDraft,
                        Public = update.IsPublic,
                        Id = recipeId.Value.ToString("N")
                    });
                }
                else
                {
                    const string sqlInsertRecipe = @"
                    insert into recipe (Id, AccountId, `Version`, DateCreated, VersionDate, Title, `Description`, DurationMinutes, Servings, Published, Public)
                    values (@Id, @AccountId, @Version, @DateCreated, @VersionDate, @Title, @Description, @DurationMinutes, @Servings, @Published, @Public)";
                    recipeId = Guid.NewGuid();
                    conn.Execute(sqlInsertRecipe, new
                    {
                        Id = recipeId.Value.ToString("N"),
                        AccountId = user.Id.ToString("N"),
                        Version = 1,
                        DateCreated = DateTime.Now,
                        VersionDate = DateTime.Now,
                        Title = update.Title,
                        Description = update.Description,
                        DurationMinutes = update.DurationMinutes,
                        Servings = update.Servings,
                        Published = !update.IsDraft,
                        Public = update.IsPublic
                    });
                }

                //TODO research tvp insert for mariadb
                const string sqlInsertStep = @"
                insert into recipe_step (Id, RecipeId, ComponentId, `Number`, Title, Body)
                values (@Id, @RecipeId, @ComponentId, @Number, @Title, @Body)";
                const string sqlInsertComponent = @"
                insert into recipe_component (Id, RecipeId, `Number`, Title, `Description`)
                values (@Id, @RecipeId, @Number, @Title, @Description)";
                const string sqlInsertStepIngredient = @"
                insert into step_ingredient (Id, RecipeId, StepId, IngredientId, `Number`, Unit, Amount, `Description`)
                select @Id, @RecipeId, @StepId, i.Id, @Number, @Unit, @Amount, @Description
                from ingredient i
                where i.`Name` = @Name";
                const string sqlInsertIngredient = @"
                insert into ingredient (Id, `Name`, `Description`, DateCreated)
                values (@Id, @Name, @Description, @DateCreated)";
                foreach (var component in update.Components)
                {
                    var componentId = component.Id ?? Guid.NewGuid();

                    conn.Execute(sqlInsertComponent, new
                    {
                        Id = componentId.ToString("N"),
                        RecipeId = recipeId.Value.ToString("N"),
                        Number = component.Number,
                        Title = component.Title,
                        Description = component.Description
                    });

                    foreach (var step in component.Steps)
                    {
                        var stepId = step.Id ?? Guid.NewGuid();
                        conn.Execute(sqlInsertStep, new
                        {
                            Id = stepId.ToString("N"),
                            RecipeId = recipeId.Value.ToString("N"),
                            ComponentId = componentId.ToString("N"),
                            Number = step.Number,
                            Title = step.Title,
                            Body = step.Body,
                        });

                        foreach (var ingredient in step.Ingredients)
                        {
                            //yeah yeah copy paste this is all slapped together
                            var ingredientId = ingredient.Id ?? Guid.NewGuid();
                            var amount = conn.Execute(sqlInsertStepIngredient, new
                            {
                                Id = ingredientId.ToString("N"),
                                RecipeId = recipeId.Value.ToString("N"),
                                StepId = stepId.ToString("N"),
                                Number = ingredient.Number,
                                Unit = ingredient.Unit,
                                Amount = ingredient.Amount,
                                Description = ingredient.Description ?? "",
                                Name = ingredient.Name
                            });
                            if (amount == 0)
                            {
                                conn.Execute(sqlInsertIngredient, new
                                {
                                    Id = Guid.NewGuid().ToString("N"),
                                    Name = ingredient.Name,
                                    Description = "",
                                    DateCreated = DateTime.Now,
                                });
                                //and do it agaaaaiiiinn lol
                                conn.Execute(sqlInsertStepIngredient, new
                                {
                                    Id = ingredientId.ToString("N"),
                                    RecipeId = recipeId.Value.ToString("N"),
                                    StepId = stepId.ToString("N"),
                                    Number = ingredient.Number,
                                    Unit = ingredient.Unit,
                                    Amount = ingredient.Amount,
                                    Description = ingredient.Description,
                                    Name = ingredient.Name
                                });
                            }
                        }
                    }
                }

                return GetRecipeWithConnection(conn, recipeId.Value);
            }
        }

        private Recipe FromData(RecipeData recipe, RecipeComponentData[] components, RecipeStepData[] steps, RecipeIngredientData[] ingredients)
        {
            return new Recipe
            {
                Id = Guid.Parse(recipe.Id),
                Author = recipe.Username,
                AuthorId = Guid.Parse(recipe.AccountId),
                DateCreated = recipe.DateCreated,
                Description = recipe.Description,
                DurationMinutes = recipe.DurationMinutes,
                IsDraft = !recipe.Published,
                IsPublic = recipe.Public,
                Servings = recipe.Servings,
                Title = recipe.Title,
                Version = recipe.Version,
                VersionDate = recipe.VersionDate,
                LastModified = recipe.VersionDate,
                Rating = 0,
                Tags = new string[] { },

                Components = components.Select(c => new Recipe.RecipeComponent
                {
                    Id = Guid.Parse(c.Id),
                    Title = c.Title,
                    Number = c.Number,
                    Description = c.Description,
                    Steps = steps.Where(s => s.ComponentId == c.Id).Select(s => new Recipe.RecipeStep
                    {
                        Id = Guid.Parse(s.Id),
                        Number = s.Number,
                        Title = s.Title,
                        Body = s.Body,
                        Ingredients = ingredients.Where(i => i.StepId == s.Id).Select(i => new Recipe.RecipeIngredient
                        {
                            Id = Guid.Parse(i.Id),
                            Number = i.Number,
                            Name = i.Name,
                            Amount = i.Amount,
                            Description = i.Description,
                            IngredientId = Guid.Parse(i.IngredientId),
                            Unit = i.Unit
                        }).ToArray(),
                    }).ToArray(),
                }).ToArray(),
            };
        }

        class RecipeData
        {
            public string Id { get; set; }
            public string AccountId { get; set; }
            public string Username { get; set; }
            public int Version { get; set; }
            public DateTime DateCreated { get; set; }
            public DateTime VersionDate { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int DurationMinutes { get; set; }
            public int Servings { get; set; }
            public bool Published { get; set; }
            public bool Public { get; set; }
        }
        class RecipeComponentData
        {
            public string Id { get; set; }
            public int Number { get; set; }
            public string RecipeId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
        }
        class RecipeStepData
        {
            public string Id { get; set; }
            public string RecipeId { get; set; }
            public string ComponentId { get; set; }
            public int Number { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }
        class RecipeIngredientData
        {
            public string Id { get; set; }
            public string StepId { get; set; }
            public string IngredientId { get; set; }
            public int Number { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
            public float Amount { get; set; }
            public string Description { get; set; }
        }
    }

    public class MockRecipeRepository : IRecipeRepository
    {
        public Recipe GetRecipe(Guid id)
        {
            return _recipeDB.FirstOrDefault(r => r.Id == id);
        }

        public RecipeListItem[] GetRecipesForUser(Guid? userId)
        {
            var mockj = new MockJournalRepository();
            return _recipeDB.Where(r => r.AuthorId == userId || r.IsPublic).Select(r =>
            {
                var journal = userId.HasValue ? mockj.GetEntriesForRecipe(userId.Value, r.Id) : new RecipeJournalListEntryDto[] { };
                return new RecipeListItem
                {
                    Id = r.Id,
                    Title = r.Title,
                    DurationMinutes = r.DurationMinutes,
                    Servings = r.Servings,
                    Author = r.Author,
                    DateCreated = r.DateCreated,
                    IsDraft = r.IsDraft,
                    IsPublic = r.IsPublic,
                    LastModified = r.VersionDate,
                    Rating = r.Rating,
                    Tags = r.Tags,
                    TotalJournalCount = 0,
                    LoggedInUserInfo = userId.HasValue ? new RecipeListItem.LoggedInUserRecipeListItemInfo
                    {
                        GoalCount = 0,
                        JournalCount = journal.Count(),
                        LatestJournalEntryDate = journal.MaxBy(j => j.Date)?.Date,
                        NoteCount = journal.Count(j => j.StickyNext && !j.NextDismissed),
                        PersonalBest = journal.MaxBy(j => j.SuccessRating)?.SuccessRating
                    } : null,
                };
            }).ToArray();
        }

        public Recipe UpdateRecipe(RecipeInputDto update, UserInfo user)
        {
            if (update.Id.HasValue)
                _recipeDB.RemoveAll(r => r.Id == update.Id);

            var updatedRecipe = new Recipe
            {
                Author = user.Username,
                AuthorId = user.Id,
                DateCreated = DateTime.Now,
                Description = update.Description,
                DurationMinutes = update.DurationMinutes,
                Id = update.Id ?? Guid.NewGuid(),
                IsDraft = update.IsDraft,
                IsPublic = update.IsPublic,
                Servings = update.Servings,
                Title = update.Title,
                Version = 1,
                VersionDate = DateTime.Now,
                Components = update.Components.Select(c => new Recipe.RecipeComponent
                {
                    Description = c.Description,
                    Id = c.Id ?? Guid.NewGuid(),
                    Number = c.Number,
                    Title = c.Title,
                    Steps = c.Steps.Select(s => new Recipe.RecipeStep
                    {
                        Body = s.Body,
                        Number = s.Number,
                        Id = s.Id ?? Guid.NewGuid(),
                        Title = s.Title,
                        Ingredients = s.Ingredients.Select(i => new Recipe.RecipeIngredient
                        {
                            Amount = i.Amount,
                            Number = i.Number,
                            Description = "this field probably needs to be pushed through the layers",
                            Id = i.Id ?? Guid.NewGuid(),
                            IngredientId = Guid.NewGuid(),
                            Name = i.Name,
                            Unit = i.Unit
                        }).ToArray(),
                    }).ToArray(),
                }).ToArray()
            };
            _recipeDB.Add(updatedRecipe);
            return updatedRecipe;
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
                        Number = 0,
                        Description = "",
                        Steps = new []
                        {
                            new Recipe.RecipeStep
                            {
                                Id = Guid.NewGuid(),
                                Title = "step 1",
                                Number = 0,
                                Body = "mix together eggs, flour, salt, milk.",
                                Ingredients = new []
                                {
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Number = 0,
                                        Name = "egg",
                                        Description = "",
                                        Amount = 4,
                                        Unit = "count",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Number = 1,
                                        Name = "flour",
                                        Description = "",
                                        Amount = .75f,
                                        Unit = "cup",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Number = 2,
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
                                Number = 1,
                                Body = "melt butter into pans, place cut apples into pans, concentric circles usually work best. I need some longer content here to test wrapping and display ugh",
                                Ingredients = new []
                                {
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Number = 0,
                                        Name = "butter",
                                        Description = "",
                                        Amount = 4,
                                        Unit = "tablespoon",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Number = 1,
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
                                Number = 2,
                                Body = "mix together sugar and cinnamon",
                                Ingredients = new []
                                {
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Number = 0,
                                        Name = "sugar",
                                        Description = "",
                                        Amount = .33f,
                                        Unit = "cup",
                                    },
                                    new Recipe.RecipeIngredient
                                    {
                                        Id = Guid.NewGuid(),
                                        IngredientId = Guid.NewGuid(),
                                        Number = 1,
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
                                Number = 3,
                                Title = "step 4",
                                Body = "pour mixture over apples",
                                Ingredients = new Recipe.RecipeIngredient[]{}
                            },
                            new Recipe.RecipeStep
                            {
                                Id = Guid.NewGuid(),
                                Number = 4,
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