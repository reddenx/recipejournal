using System;
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
    [Authorize]
    [Route("api/shopping")]
    public class ShoppingController : Controller
    {
        [HttpGet]
        public ActionResult<ShoppingListDto> GetShoppingList()
            => throw new NotImplementedException();
        public class ShoppingListDto
        {
            public Guid[] AssociatedRecipes;
            public IngredientItemDto[] Ingredients;

            public class IngredientItemDto
            {
                public Guid IngredientId;
                public float AmountRequired;
                public bool Completed;
                public Guid SourceRecipe;
            }
            // public class IngredientUnitDto //this would be on the ingredient if looked up
            // {
            //     public string Type;
            //     public bool Fractional;
            //     public float Amount;
            // }
        }

        [HttpPut]
        public IStatusCodeActionResult UpdateShoppingList([FromBody] UpdateShoppingListDto dto)
            => throw new NotImplementedException();
        public class UpdateShoppingListDto { }
    }
}