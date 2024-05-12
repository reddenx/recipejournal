using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using RecipeJournalApi.Infrastructure;
using SMT.Utilities.Logging;

namespace RecipeJournalApi.Controllers
{


    [Authorize]
    [Route("api/v1/shopping")]
    public class ShoppingController : Controller
    {
        private readonly IShoppingRepository _shoppingRepo;
        private readonly ITraceLogger _logger;
        private readonly IRecipeRepository _recipeRepo;

        public ShoppingController(IShoppingRepository shoppingRepo, ITraceLogger logger, IRecipeRepository recipeRepo)
        {
            _shoppingRepo = shoppingRepo;
            _recipeRepo = recipeRepo;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<ShoppingListDto> GetShoppingList()
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            var list = _shoppingRepo.GetUserShoppingList(user.Id);

            return new ShoppingListDto()
            {
                Recipes = list.Recipes.Select(r => new ShoppingListRecipeDto
                {
                    Id = r.Id,
                    Scale = r.Scale,
                    Title = r.Title,
                    Ingredients = r.Ingredients.Select(i => new ShoppingListIngredientDto
                    {
                        Id = i.Id,
                        Amount = i.Amount,
                        Name = i.Name,
                        Unit = i.Unit,
                    }).ToArray(),
                }).ToArray(),
                GatheredIds = list.GatheredIngredients.Select(i => i.Id).ToArray(),
                NonrecipeIngredients = list.NonrecipeIngredients.Select(nr => new NonrecipeIngredientDto
                {
                    Id = nr.IngredientId,
                    Name = nr.Name,
                    Amount = nr.Amount,
                }).ToArray()
            };
        }
        public class ShoppingListDto
        {
            public ShoppingListRecipeDto[] Recipes { get; set; }
            public Guid[] GatheredIds { get; set; }
            public NonrecipeIngredientDto[] NonrecipeIngredients { get; set; }
        }
        public class ShoppingListRecipeDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public float Scale { get; set; }
            public ShoppingListIngredientDto[] Ingredients { get; set; }
        }
        public class ShoppingListIngredientDto
        {
            public Guid Id { get; set; }
            public string Unit { get; set; }
            public string Name { get; set; }
            public float Amount { get; set; }
        }

        [HttpPut]
        public IStatusCodeActionResult UpdateShoppingList([FromBody] UpdateShoppingListDto dto)
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);

            var nonrecipeIngredients = new List<NonrecipeIngredient>(
                dto.NonrecipeIngredients?
                    .Where(nr => nr.Id.HasValue)
                    .Select(nr => new NonrecipeIngredient()
                    {
                        IngredientId = nr.Id.Value,
                        Amount = nr.Amount,
                    }) ?? new NonrecipeIngredient[] { });

            var nonrecipeIngredientsWithoutIds = dto.NonrecipeIngredients?.Where(nr => !nr.Id.HasValue).ToArray() ?? new NonrecipeIngredientDto[] { };
            foreach (var iggyWithoutIddy in nonrecipeIngredientsWithoutIds)
            {
                var ingredient = _recipeRepo.GetIngredientByName(iggyWithoutIddy.Name);
                if (ingredient == null)
                {
                    ingredient = _recipeRepo.CreateIngredient(iggyWithoutIddy.Name, "");
                }

                nonrecipeIngredients.Add(new NonrecipeIngredient
                {
                    Amount = iggyWithoutIddy.Amount,
                    IngredientId = ingredient.Id
                });
            }

            var success = _shoppingRepo.UpdateShoppingList(
                userId: user.Id,
                recipeIds: dto.RecipeScales
                    .Where(r => r.Scale > 0)
                    .Select(r => new ShoppingRecipeScale
                    {
                        Id = r.Id,
                        Scale = r.Scale
                    }).ToArray(),
                gatheredIngredientIds: dto.GatheredIds,
                nonrecipeIngredients: nonrecipeIngredients.ToArray()
            );

            if (!success)
            {
                _logger.Error("failed to update shopping list", dto, user?.Username);
                return StatusCode(400);
            }
            return StatusCode(204);
        }
        public class UpdateShoppingListDto
        {
            public ShoppingRecipeScaleDto[] RecipeScales { get; set; }
            public Guid[] GatheredIds { get; set; }
            public NonrecipeIngredientDto[] NonrecipeIngredients { get; set; }
        }
        public class ShoppingRecipeScaleDto
        {
            public Guid Id { get; set; }
            public float Scale { get; set; }
        }
        public class NonrecipeIngredientDto
        {
            public Guid? Id { get; set; }
            public string Name { get; set; }
            public string Amount { get; set; }
        }
    }
}