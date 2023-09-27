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
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult GetLoggedInUserInfo()
        {
            throw new NotImplementedException();
        }

        public class CreateUserDto
        {
            public string Username { get; set; }
        }

        [HttpPost("")]
        public IActionResult CreateUser([FromBody] CreateUserDto input)
        {
            throw new NotImplementedException();
        }

        public class LoginDto
        {
            public string Username { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            //TODO validate login

            var authProperties = new AuthenticationProperties();
            var claims = new[]
            {
                new Claim("username", login.Username),
                new Claim("access-level", "admin") 
                //todo: 
                // anon: readonly recipes
                // user: normal user, signed up anonymously, allowed to do normal user stuff like create recipes
                // contributor: user + modify categories and ingredients
                // admin: user/contributor + modify users
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return StatusCode(204);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return StatusCode(204);
        }
    }
}