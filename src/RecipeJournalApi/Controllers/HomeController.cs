using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RecipeJournalApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/{page?}/{id?}")]
        public IActionResult Index(string page, string id) 
        {
            return File("index.html", "text/html");
        }
    }
}