using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SMT.Utilities.Logging;
using System;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using RecipeJournalApi.Infrastructure;

namespace RecipeJournalApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        private static readonly IMemoryCache _preauthCache = new MemoryCache(new MemoryCacheOptions());
        public class KeyCacheInfo
        {
            public string PreAuthKey { get; set; }
            public string PostAuthKey { get; set; }
        }

        private readonly ITraceLogger _logger;
        private readonly IAuthenticationUtility _auth;
        private readonly IUserRepository _userRepo;

        public HomeController(ITraceLogger logger, IAuthenticationUtility auth, IUserRepository userRepo)
        {
            _logger = logger;
            _auth = auth;
            _userRepo = userRepo;
        }

        [HttpGet("/{page?}/{id?}")]
        [ResponseCache(Duration = 60 * 60 * 24 * 30, Location = ResponseCacheLocation.Client)]
        public IActionResult Index(string page, string id)
        {
            _logger.Debug("home page accessed", $"page: {page}", $"id: {id}");
            return File("index.html", "text/html");
        }

        [HttpGet("api/v1/version")]
        public IActionResult GetVersion()
        {
            var versionString = GetType().Assembly.GetName().Version.ToString();
            return StatusCode(200, versionString);
        }


        [HttpGet("login")]
        public async Task<IActionResult> LoginRedirect()
        {
            var preauthInfo = await _auth.GetPreauthData();
            _preauthCache.Set(preauthInfo.Code, new KeyCacheInfo
            {
                PreAuthKey = preauthInfo.Key
            }, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
            });

            return Redirect(preauthInfo.Redirect);
        }
        [HttpPost("integrationauth/{code}")]
        public IActionResult KeyPost([FromRoute] string code, [FromBody] AuthInputKeyDto dto)
        {
            if (_preauthCache.TryGetValue<KeyCacheInfo>(code, out var preauthInfo))
                preauthInfo.PostAuthKey = dto.Key;
            return StatusCode(204);
        }
        public class AuthInputKeyDto
        {
            public string Key { get; set; }
        }

        [HttpGet("login/{code}")]
        public async Task<IActionResult> LoginRedirect(string code)
        {
            if (_preauthCache.TryGetValue<KeyCacheInfo>(code, out var preauthInfo))
            {
                var session = await _auth.StartSession(preauthInfo.PreAuthKey, preauthInfo.PostAuthKey);
                if (session == null)
                    return Redirect("/");

                //do login stuff
                var user = _userRepo.GetUserByIntegration(session.AccountId);
                if (user == null)
                {
                    var accountId = Guid.NewGuid();
                    _userRepo.CreateUser(accountId, session.Username, session.AccountId);
                    user = _userRepo.GetUserByAccountId(accountId);
                }
                else if (user.Username != session.Username)
                {
                    _userRepo.UpdateUsername(user.Id, session.Username);
                }

                //TODO probably want to randomize a delay to prevent distinguishing these 400s from a malicious user
                if (user == null)
                {
                    _logger.Error("failed to find or create user", code, session.SessionId, session.AccountId);
                    return Redirect("/");
                }

                var userInfo = new UserInfo
                {
                    Id = user.Id,
                    AccessLevel = user.AccessLevel,
                    Username = session.Username
                };

                var authProperties = new AuthenticationProperties()
                {
                    AllowRefresh = false,
                    ExpiresUtc = session.ExpirationUtc,
                    IssuedUtc = DateTime.UtcNow,
                };
                var claims = userInfo.ToClaims();
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                _preauthCache.Remove(code);

                _logger.Debug("user logged in", user.Id);
                return Redirect("/");
            }

            return Redirect("/");
        }
    }
}