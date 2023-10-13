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
using RecipeJournalApi.Infrastructure;

namespace RecipeJournalApi.Controllers
{
    

    [Authorize]
    [Route("api/v1/shopping")]
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
                GatheredIds = list.GatheredIngredients.Select(i => i.Id).ToArray()
            };
        }
        public class ShoppingListDto
        {
            public ShoppingListRecipeDto[] Recipes { get; set; }
            public Guid[] GatheredIds { get; set; }
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
            var success = _shoppingRepo.UpdateShoppingList(user.Id, dto.RecipeScales
                .Where(r => r.Scale > 0)
                .Select(r => new ShoppingRecipeScale
                {
                    Id = r.Id,
                    Scale = r.Scale
                }).ToArray(), dto.GatheredIds);
            return StatusCode(success ? 204 : 400);
        }
        public class UpdateShoppingListDto
        {
            public ShoppingRecipeScale[] RecipeScales { get; set; }
            public Guid[] GatheredIds { get; set; }
        }
        public class ShoppingRecipeScaleDto
        {
            public Guid Id { get; set; }
            public float Scale { get; set; }
        }
    }
}