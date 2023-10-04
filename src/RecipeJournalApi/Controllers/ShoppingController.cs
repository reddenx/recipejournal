using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;

namespace RecipeJournalApi.Controllers
{
    public interface IShoppingRepository
    {
        ShoppingList GetUserShoppingList(Guid userId);
        bool UpdateShoppingList(Guid userId, Guid[] recipeIds, Guid[] gatheredIngredientIds);
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

    [Authorize]
    [Route("api/shopping")]
    public class ShoppingController : Controller
    {
        private readonly IShoppingRepository _shoppingRepo;

        public ShoppingController(IShoppingRepository shoppingRepo)
        {
            _shoppingRepo = shoppingRepo;
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
                Gathered = list.GatheredIngredients.Select(i => i.Name).ToArray()
            };
        }
        public class ShoppingListDto
        {
            public ShoppingListRecipeDto[] Recipes { get; set; }
            public string[] Gathered { get; set; }
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
            var success = _shoppingRepo.UpdateShoppingList(user.Id, dto.RecipeIds, dto.GatheredIngredientIds);
            return StatusCode(success ? 204 : 400);
        }
        public class UpdateShoppingListDto
        {
            public Guid[] RecipeIds { get; set; }
            public Guid[] GatheredIngredientIds { get; set; }
        }
    }
}