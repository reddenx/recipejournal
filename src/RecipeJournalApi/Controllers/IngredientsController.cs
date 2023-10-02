using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RecipeJournalApi.Controllers
{
    [Authorize]
    [Route("api/ingredients")]
    public class IngredientsController : Controller
    {
        // [HttpGet("")]
        // public ActionResult<IngredientListDto[]> GetIngredientList()
        // {
        //     throw new NotImplementedException();
        // }
        // public class IngredientListDto{}

        [HttpGet("{id}")]
        public ActionResult<IngredientDetailsDto> GetIngredient(string id)
        {
            throw new NotImplementedException();
        }
        public class IngredientDetailsDto {}

        [HttpGet("")]
        public ActionResult<IngredientDetailsDto[]> GetManyIngredients([FromQuery]string[] ids)
            => throw new NotImplementedException();

        [HttpPut("{id}")]
        public IActionResult UpdateIngredientDetails([FromBody] UpdateIngredientDto dto)
        {
            throw new NotImplementedException();
        }
        public class UpdateIngredientDto {}

        [HttpPut("{id}/categories")]
        public IActionResult SetIngredientCategories([FromBody] Guid[] ingredientIds)
            => throw new NotImplementedException();
        
        [HttpGet("/categories")]
        public ActionResult<IngredientCategoryDto[]> GetIngredientCategories()
            => throw new NotImplementedException();
        public class IngredientCategoryDto {}

        [HttpPut("/categories/{id}")]
        public IActionResult UpdateIngredientCategory(string categoryId) 
            => throw new NotImplementedException();
    }
}