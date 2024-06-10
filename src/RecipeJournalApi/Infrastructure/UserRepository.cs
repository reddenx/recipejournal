using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using SMT.Utilities.Logging;

namespace RecipeJournalApi.Infrastructure
{
    public interface IUserRepository
    {
        UserData GetUserByAccountId(Guid accountId);
        UserData GetUserByIntegration(string integrationAccountId);
        UserData CreateUser(Guid accountId, string integrationAccountId);
    }

    public class UserData
    {
        public UserData(Guid id, string accessLevel, string integrationAccountId)
        {
            Id = id;
            AccessLevel = accessLevel;
            IntegrationAccountId = integrationAccountId;
        }

        public Guid Id { get; private set; }
        public string AccessLevel { get; private set; }
        public string IntegrationAccountId { get; private set; }
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

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ITraceLogger _logger;

        public UserRepository(IDbConfig config, ITraceLogger logger)
        {
            _connectionString = config.ConnectionString;
            _logger = logger;
        }

        public UserData CreateUser(Guid accountId, string integrationAccountId)
        {
            try
            {
                var sql = @"";
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                _logger.Error("failed to create user", e, accountId, integrationAccountId);
                throw;
            }
        }
        public UserData GetUserByAccountId(Guid accountId)
        {
            try
            {
                var sql = @"
                    select 
                        a.Id,
                        a.IntegrationAccountId,
                        a.PermissionsRole
                    from account a
                    where a.Id = @AccountId";

                using (var conn = new MySqlConnection(_connectionString))
                {
                    var data = conn.Query<UserDataRow>(sql, new
                    {
                        AccountId = accountId.ToString("N")
                    }).FirstOrDefault();

                    if (data == null)
                        return null;

                    return new UserData(
                        Guid.Parse(data.Id),
                        data.PermissionsRole,
                        data.IntegrationAccountId);
                }
            }
            catch (Exception e)
            {
                _logger.Error("failed to get user", e, accountId);
                throw;
            }
        }
        public UserData GetUserByIntegration(string integrationAccountId)
        {
            try
            {
                var sql = @"
                    select 
                        a.Id,
                        a.IntegrationAccountId,
                        a.PermissionsRole
                    from account a
                    where a.IntegrationAccountId = @IntegrationAccountId";

                using (var conn = new MySqlConnection(_connectionString))
                {
                    var data = conn.Query<UserDataRow>(sql, new
                    {
                        IntegrationAccountId = integrationAccountId
                    }).FirstOrDefault();

                    if (data == null)
                        return null;

                    return new UserData(
                        Guid.Parse(data.Id),
                        data.PermissionsRole,
                        data.IntegrationAccountId);
                }
            }
            catch (Exception e)
            {
                _logger.Error("failed to get user by integration", e, integrationAccountId);
                throw;
            }
        }

        class UserDataRow
        {
            public string Id { get; set; }
            public string IntegrationAccountId { get; set; }
            public string PermissionsRole { get; set; }
        }
    }

    public class MockUserRepository : IUserRepository
    {
        public static readonly Guid MOCK_USER = Guid.Parse("00000000000000000000000000000001");

        private static readonly List<MockUser> _users = new List<MockUser>();
        class MockUser
        {
            public Guid Id { get; set; }
            public string IntegrationAccountId { get; set; }
            public string Role { get; set; }
        }

        public UserData CreateUser(Guid accountId, string integrationAccountId)
        {
            var user = new MockUser
            {
                IntegrationAccountId = integrationAccountId,
                Role = "user",
                Id = accountId,
            };

            //first mock user created is admin with some other things hooked to it
            if (_users.Count == 0)
            {
                user.Id = MOCK_USER;
                user.Role = "admin";
            }

            _users.Add(user);
            return new UserData(user.Id, user.Role, user.IntegrationAccountId);
        }

        public UserData GetUserByAccountId(Guid accountId)
        {
            var user = _users.FirstOrDefault(u => u.Id == accountId);
            if (user == null)
                return null;
            return new UserData(user.Id, user.Role, user.IntegrationAccountId);
        }

        public UserData GetUserByIntegration(string integrationAccountId)
        {
            var user = _users.FirstOrDefault(u => u.IntegrationAccountId == integrationAccountId);
            if (user == null)
                return null;
            return new UserData(user.Id, user.Role, user.IntegrationAccountId);
        }
    }
}