using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SMT.Utilities.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace RecipeJournalApi.Infrastructure
{
    public interface IAuthenticationProxy
    {
        Task<bool> AuthenticateAccount(Guid accountId, string secret);
    }

    public interface IAuthenticationProxyConfiguration
    {
        string AccountServerUrl { get; }
        string AccountIntegrationName { get; }
    }
    public class AuthenticationProxy : IAuthenticationProxy
    {
        private readonly string _accountServerUrl;
        private readonly string _integrationName;
        private IHttpClientFactory _clientFactory;
        private readonly ITraceLogger _logger;

        public AuthenticationProxy(IAuthenticationProxyConfiguration config, IHttpClientFactory clientFactory, ITraceLogger logger)
        {
            _accountServerUrl = config.AccountServerUrl;
            _integrationName = config.AccountIntegrationName;
            _clientFactory = clientFactory;
            _logger = logger;
        }

        public async Task<bool> AuthenticateAccount(Guid accountId, string secret)
        {
            var authDto = new AuthenticateDto
            {
                IntegrationIdentifier = accountId.ToString("N"),
                IntegrationName = _integrationName,
                Secret = secret,
            };

            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request: new HttpRequestMessage(HttpMethod.Post, $"{_accountServerUrl}/api/v1/account/authenticate")
                {
                    Content = new StringContent(JsonSerializer.Serialize(authDto), Encoding.UTF8, Application.Json)
                });
                _logger.Debug("auth request result", response.StatusCode);
                return response.IsSuccessStatusCode;
            }
            catch(Exception e)
            {
                _logger.Error("failure authenticating account", e, $"accountid: {accountId}");
                return false;
            }
        }
        class AuthenticateDto
        {
            public string IntegrationIdentifier { get; set; }
            public string IntegrationName { get; set; }
            public string Secret { get; set; }
        }
    }

    public class MockAuthProxy : IAuthenticationProxy
    {
        public Task<bool> AuthenticateAccount(Guid accountId, string secret)
        {
            return Task.FromResult(true);
        }
    }
}