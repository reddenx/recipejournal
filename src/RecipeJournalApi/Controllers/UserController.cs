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
        private readonly IAuthenticationProxy _authProxy;

        public UserController(IUserRepository userRepo, IAuthenticationProxy authProxy, ITraceLogger logger)
        {
            _userRepo = userRepo;
            _authProxy = authProxy;
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

        [HttpPost("")]
        public IActionResult CreateUser([FromBody] CreateUserDto input)
        {
            _logger.Error("attempted to create user, this endpoint is disabled", input.Username);
            throw new NotImplementedException();
        }

        public class LoginDto
        {
            public string Username { get; set; }
            public string Secret {get;set;}
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var user = _userRepo.GetUser(login.Username);

            //TODO probably want to randomize a delay to prevent distinguishing these 400s from a malicious user
            if(user == null)
            {
                _logger.Info("no user found by username", login.Username);
                return StatusCode(400);
            }
            
            var areValidCredentials = await _authProxy.AuthenticateAccount(user.Id, login.Secret);
            if(!areValidCredentials)
            {
                _logger.Info("user entered bad credentials", login.Username);
                return StatusCode(400);
            }
            
            var authProperties = new AuthenticationProperties();
            var claims = user.ToClaims();
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            _logger.Debug("user logged in", login.Username);
            return StatusCode(204);
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