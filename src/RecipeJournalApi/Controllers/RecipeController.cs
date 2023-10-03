using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RecipeJournalApi.Controllers
{
    [ApiController]
    [Route("api/v1/recipes")]
    public class RecipeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRecipeRepository _recipeRepo;

        public RecipeController(ILogger<HomeController> logger, IRecipeRepository recipeRepo)
        {
            _logger = logger;
            _recipeRepo = recipeRepo;
        }

        [HttpGet("")]
        public ActionResult<RecipeListItemDto[]> GetRecipeList()
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            var recipes = _recipeRepo.GetRecipesForUser(user?.Id);
            return recipes.Select(r => new RecipeListItemDto
            {
                Id = r.Id,
                DurationMinutes = r.DurationMinutes,
                Servings = r.Servings,
                Title = r.Title
            }).ToArray();
        }
        public class RecipeListItemDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int? DurationMinutes { get; set; }
            public int? Servings { get; set; }
        }

        [HttpGet("{id}")]
        public ActionResult<RecipeDto> GetRecipe([FromRoute] Guid id)
        {
            var recipe = _recipeRepo.GetRecipe(id);
            if (recipe == null)
                return StatusCode(404);
            return RecipeDto.FromDataType(recipe);
        }
        public class RecipeDto
        {
            public static RecipeDto FromDataType(Recipe recipe)
            {
                return new RecipeDto
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Author = recipe.Author,
                    Description = recipe.Description,
                    DurationMinutes = recipe.DurationMinutes,
                    IsDraft = !recipe.IsDraft,
                    IsPublic = !recipe.IsPublic,
                    Servings = recipe.Servings,
                    Components = recipe.Components.Select(c => new RecipeDto.RecipeComponentDto
                    {
                        Id = c.Id,
                        Description = c.Description,
                        Title = c.Title,
                        Steps = c.Steps.Select(s => new RecipeDto.RecipeStepDto
                        {
                            Id = s.Id,
                            Title = s.Title,
                            Body = s.Body,
                            Ingredients = s.Ingredients.Select(i => new RecipeDto.RecipeIngredientDto
                            {
                                Id = i.Id,
                                Name = i.Name,
                                Amount = i.Amount,
                                Unit = i.Unit
                            }).ToArray()
                        }).ToArray()
                    }).ToArray()
                };
            }

            public Guid? Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Author { get; set; }
            public int? DurationMinutes { get; set; }
            public int? Servings { get; set; }
            public bool IsDraft { get; set; }
            public bool IsPublic { get; set; }
            public RecipeComponentDto[] Components { get; set; }

            public class RecipeComponentDto
            {
                public Guid? Id { get; set; }
                public string Title { get; set; }
                public string Description { get; set; }
                public RecipeStepDto[] Steps { get; set; }
            }
            public class RecipeStepDto
            {
                public Guid? Id { get; set; }
                public string Title { get; set; }
                public string Body { get; set; }
                public RecipeIngredientDto[] Ingredients { get; set; }
            }
            public class RecipeIngredientDto
            {
                public Guid? Id { get; set; }
                public string Name { get; set; }
                public string Unit { get; set; }
                public float Amount { get; set; }
            }
        }

        [HttpPut("")]
        public ActionResult<RecipeDto> UpdateRecipe(RecipeDto recipeDto)
        {
            var updated = _recipeRepo.UpdateRecipe(recipeDto);
            var dto = RecipeDto.FromDataType(updated);
            return dto;
        }
    }
}