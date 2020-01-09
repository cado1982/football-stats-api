using Microsoft.Extensions.Configuration;

namespace FootballStatsApi.Web.Helpers
{
    public class ConfigurationHelper
    {
        private readonly IConfiguration _config;

        public ConfigurationHelper(IConfiguration config)
        {
            _config = config;
        }

        public string ApiBaseUrl { get { return _config[nameof(ApiBaseUrl)]; } }
    }
}