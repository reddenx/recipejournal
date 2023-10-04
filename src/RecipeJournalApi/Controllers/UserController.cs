using System;
using System.Data.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipeJournalApi.Infrastructure;

namespace RecipeJournalApi.Controllers
{
    [Authorize]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepo;

        public UserController(ILogger<UserController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        [HttpGet("")]
        public IActionResult GetLoggedInUserInfo()
        {
            var user = UserInfo.FromClaimsPrincipal(this.User);
            if (user != null)
            {
                return Json(new
                {
                    UserId = user.Id.ToString("N"),
                    Username = user.Username,
                    AccessLevel = user.AccessLevel,
                });
            }
            return StatusCode(404);
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
            var user = _userRepo.GetUser(login.Username);

            var authProperties = new AuthenticationProperties();
            var claims = user.ToClaims();
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