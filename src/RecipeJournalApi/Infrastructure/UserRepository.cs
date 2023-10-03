using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RecipeJournalApi.Controllers
{
    public interface IUserRepository
    {
        UserInfo GetUser(string username);
        UserInfo GetUser(Guid id);
        //create user
        //update user
        //deactivate user
    }

    public class UserInfo
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string AccessLevel { get; set; }

        public static UserInfo FromClaimsPrincipal(ClaimsPrincipal user)
        {
            try
            {
                return new UserInfo
                {
                    Id = Guid.Parse(user.Claims.FirstOrDefault(c => c.Type == "userid")?.Value),
                    AccessLevel = user.Claims.FirstOrDefault(c => c.Type == "access-level")?.Value,
                    Username = user.Claims.FirstOrDefault(c => c.Type == "username")?.Value
                };
            }
            catch
            {
                return null;
            }
        }

        public Claim[] ToClaims()
        {
            return new[]
            {
                new Claim("userid", this.Id.ToString("N")),
                new Claim("username", this.Username),
                new Claim("access-level", this.AccessLevel),
                
                //todo: 
                // anon: readonly recipes
                // user: normal user, signed up anonymously, allowed to do normal user stuff like create recipes
                // contributor: user + modify categories and ingredients
                // admin: user/contributor + modify users
            };
        }
    }

    public class MockUserRepository : IUserRepository
    {
        public static readonly Guid MOCK_USER = Guid.Parse("00000000000000000000000000000001");

        public UserInfo GetUser(string username)
        {
            if (username == "sean")
            {
                return new UserInfo
                {
                    Username = "sean",
                    Id = MOCK_USER,
                    AccessLevel = "admin",
                };
            }
            return null;
        }

        public UserInfo GetUser(Guid id)
        {
            if (id == MOCK_USER)
            {
                return GetUser("sean");
            }
            return null;
        }
    }
}