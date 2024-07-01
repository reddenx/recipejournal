using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipeJournalApi.Infrastructure
{
    public interface IAuthenticationUtility
    {
        Task<PreauthData> GetPreauthData();
        Task<SessionData> StartSession(string preAuthKey, string postAuthKey);
        Task<AccountData> GetAccount(string sessionId);
        Task<bool> EndSession(string sessionId);
    }
    public class PreauthData
    {
        public PreauthData(string code, string key, string redirect)
        {
            Code = code;
            Key = key;
            Redirect = redirect;
        }

        public string Code { get; private set; }
        public string Key { get; private set; }
        public string Redirect { get; private set; }
    }
    public class SessionData
    {
        public SessionData(string sessionId, string accountId, string username, DateTime expirationUtc)
        {
            SessionId = sessionId;
            AccountId = accountId;
            Username = username;
            ExpirationUtc = expirationUtc;
        }

        public string SessionId { get; private set; }
        public string AccountId { get; private set; }
        public string Username { get; private set; }
        public DateTime ExpirationUtc { get; private set; }
    }
    public class AccountData
    {
        public AccountData(string accountId, string username)
        {
            AccountId = accountId;
            Username = username;
        }

        public string AccountId { get; private set; }
        public string Username { get; private set; }
    }


    public interface IAuthenticationConfiguration
    {
        string Domain { get; }
        string AuthBaseUrl { get; }
    }
    public class AuthenticationUtility : IAuthenticationUtility
    {
        private readonly IAuthenticationConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;

        public AuthenticationUtility(IAuthenticationConfiguration config, IHttpClientFactory clientFactory)
        {
            _config = config;
            _clientFactory = clientFactory;
        }

        public async Task<PreauthData> GetPreauthData()
        {
            var client = _clientFactory.CreateClient();

            var jsonContent = JsonContent.Create(new PreAuthStartDto
            {
                Domain = _config.Domain,
                KeyEndpoint = "integrationauth",
                Redirect = "login"
            }, options: new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var response = await client.PostAsync($"{_config.AuthBaseUrl}/integrations/v1/preauth", jsonContent);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<PreAuthResultDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return new PreauthData(dto.Code, dto.Key, dto.Redirect);
        }

        public async Task<AccountData> GetAccount(string sessionId)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync($"{_config.AuthBaseUrl}/integrations/v1/account");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<IntegrationAccountDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return new AccountData(dto.AccountId, dto.Username);
        }

        public async Task<SessionData> StartSession(string preAuthKey, string postAuthKey)
        {
            if (string.IsNullOrWhiteSpace(preAuthKey) || string.IsNullOrWhiteSpace(postAuthKey))
                return null;

            var client = _clientFactory.CreateClient();

            var jsonContent = JsonContent.Create(new IntegrationAccountKeysDto
            {
                PostAuthKey = postAuthKey,
                PreAuthKey = preAuthKey,
            }, options: new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var response = await client.PostAsync($"{_config.AuthBaseUrl}/integrations/v1/session", jsonContent);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<IntegrationSessionDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return new SessionData(dto.SessionId, dto.Account.AccountId, dto.Account.Username, dto.ExpirationUtc);
        }

        public async Task<bool> EndSession(string sessionId)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.DeleteAsync($"{_config.AuthBaseUrl}/integrations/v1/session");

            return response.IsSuccessStatusCode;
        }

        class IntegrationSessionDto
        {
            public string SessionId { get; set; }
            public DateTime ExpirationUtc { get; set; }
            public IntegrationAccountDto Account { get; set; }
        }
        class IntegrationAccountKeysDto
        {
            public string PreAuthKey { get; set; }
            public string PostAuthKey { get; set; }
        }

        class IntegrationAccountDto
        {
            public string AccountId { get; set; }
            public string Username { get; set; }
        }
        class PreAuthStartDto
        {
            public string Domain { get; set; }
            public string KeyEndpoint { get; set; }
            public string Redirect { get; set; }
            public string RedirectQueryParam { get; set; }
        }
        class PreAuthResultDto
        {
            public string Code { get; set; }
            public string Key { get; set; }
            public string Redirect { get; set; }
        }
    }

    public class MockAuthenticationUtility : IAuthenticationUtility
    {
        private static readonly List<MockSession> _sessions = new List<MockSession>();
        private static readonly List<MockPreauth> _preauths = new List<MockPreauth>();

        private readonly IAuthenticationConfiguration _config;

        public MockAuthenticationUtility(IAuthenticationConfiguration config)
        {
            _config = config;
        }

        class MockPreauth
        {
            public string Code { get; set; }
            public string Key { get; set; }
            public string Redirect { get; set; }
            public string PreKey { get; set; }
        }

        class MockSession
        {
            public string SessionId { get; set; }
            public AccountData AccountData { get; set; }
        }

        public async Task<bool> EndSession(string sessionId)
        {
            _sessions.RemoveAll(s => s.SessionId == sessionId);
            return true;
        }

        public async Task<AccountData> GetAccount(string sessionId)
        {
            return _sessions.FirstOrDefault(s => s.SessionId == sessionId)?.AccountData;
        }

        public async Task<PreauthData> GetPreauthData()
        {
            var code = Guid.NewGuid().ToString("N");
            var data = new MockPreauth()
            {
                Code = code,
                Key = Guid.NewGuid().ToString("N"),
                PreKey = Guid.NewGuid().ToString("N"),
                Redirect = $"http://{_config.Domain}/login/{code}"
            };
            _preauths.Add(data);
            return new PreauthData(data.Code, data.Key, data.Redirect);
        }

        public async Task<SessionData> StartSession(string preAuthKey, string postAuthKey)
        {
            var data = _preauths.FirstOrDefault(p => p.PreKey == preAuthKey);
            var id = Guid.NewGuid().ToString("N");
            var session = new MockSession()
            {
                SessionId = Guid.NewGuid().ToString("N"),
                AccountData = new AccountData(id, $"user-{id.Substring(0, 3)}")
            };
            return new SessionData(session.SessionId, session.AccountData.AccountId, session.AccountData.Username, DateTime.UtcNow.AddMonths(3));
        }
    }
}
