using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMT.Utilities.Logging;

namespace RecipeJournalApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly ITraceLogger _logger;

        public HomeController(ITraceLogger logger)
        {
            _logger = logger;
        }

        [HttpGet("/{page?}/{id?}")]
        public IActionResult Index(string page, string id) 
        {
            _logger.Debug("home page accessed", $"page: {page}", $"id: {id}");
            return File("index.html", "text/html");
        }
    }
}