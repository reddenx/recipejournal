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