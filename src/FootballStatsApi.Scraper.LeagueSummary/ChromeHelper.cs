using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FootballStatsApi.Scraper.LeagueSummary
{

    public class ChromeHelper
    {
        private static readonly HttpClient _client = new HttpClient();
        private readonly ILogger<ChromeHelper> _logger;
        private readonly ChromeSettings _chromeSettings;
        private readonly IConfiguration _configuration;

        public ChromeHelper(ILogger<ChromeHelper> logger, ChromeSettings chromeSettings, IConfiguration configuration)
        {
            _logger = logger;
            _chromeSettings = chromeSettings;
            _configuration = configuration;
        }

        public async Task<ConnectOptions> GetConnectOptionsAsync()
        {
            var options = new ConnectOptions
            {
                BrowserWSEndpoint = await GetWSEndpoint()
            };

            return options;
        }

        private async Task<string> GetWSEndpoint()
        {
            try
            {
                var uri = new Uri($"http://{_chromeSettings.Host}:{_chromeSettings.Port}/json/version");
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                request.Headers.Add("Host", $"localhost:{_chromeSettings.Port}");
                
                _logger.LogTrace($"Getting chrome ws endpoint from {uri}");

                var response = await _client.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var metadata = JsonConvert.DeserializeObject<ChromeMetadata>(json);

                // Localhost is changed to chrome in prod because we're running under a docker compose network
                // where chrome refers to the service name
                var endpoint = metadata.WSEndpoint;
                var isDev = _configuration["DOTNET_ENVIRONMENT"] == "Development";

                if (!isDev)
                {
                    endpoint = endpoint.Replace("localhost", "chrome");
                }
                
                _logger.LogTrace($"Found chrome ws endpoint: {endpoint}");

                return endpoint;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Unable to connect to chrome debugging metadata endpoint");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to get WS endpoint to connect to chrome");
                throw;
            }
        }
    }
}
