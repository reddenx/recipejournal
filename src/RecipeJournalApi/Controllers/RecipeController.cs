using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RecipeJournalApi.Controllers
{
    [ApiController]
    [Route("api/recipes")]
    public class RecipeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public RecipeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public ActionResult<RecipeListItemDto[]> GetRecipeList()
        {
            throw new NotImplementedException();
        }
        public class RecipeListItemDto { }

        [HttpGet("{id}")]
        public ActionResult<RecipeDto> GetRecipe([FromRoute] Guid id)
        {
            throw new NotImplementedException();
        }
        public class RecipeDto { }

        [HttpPost("")]
        public IActionResult CreateRecipe(CreateRecipeDto recipeDto)
        {
            throw new NotImplementedException();
        }
        public class CreateRecipeDto { }

        [HttpPut("")]
        public IActionResult UpdateRecipe(RecipeDto recipeDto)
        {
            throw new NotImplementedException();
        }
    }
}