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
using SMT.Utilities.Logging;

namespace RecipeJournalApi.Controllers
{
    [Authorize]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly ITraceLogger _logger;
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo, ITraceLogger logger)
        {
            _userRepo = userRepo;
            _logger = logger;
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
            _logger.Debug("attempted to get logged in user, but none was present");
            return StatusCode(404);
        }

        public class CreateUserDto
        {
            public string Username { get; set; }
            public string Secret {get;set;}
        }

        [AllowAnonymous]
        [HttpPost("")]
        public IActionResult CreateUser([FromBody] CreateUserDto input)
        {
            _logger.Error("attempted to create user, this endpoint is disabled", input.Username);
            return StatusCode(401, "you are not authorized to use this endpoint");
        }

        public class LoginDto
        {
            public string Username { get; set; }
            public string Secret {get;set;}
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _logger.Debug("user logged out");
            return StatusCode(204);
        }
    }
}