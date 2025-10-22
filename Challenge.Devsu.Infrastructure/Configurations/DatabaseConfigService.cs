using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Challenge.Devsu.Infrastructure.Configurations
{
    public class DatabaseConfigService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DatabaseConfigService> _logger;
        public DatabaseConfigService(IConfiguration configuration,
            ILogger<DatabaseConfigService> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }

        public string GetConnectionString()
        {
            var dbHost = _configuration["HOST"];
            var dbDatabase = _configuration["DATABASE"];
            var dbPort = _configuration["PORT"];
            var dbUser = _configuration["USER_ID"];
            var dbPassword = _configuration["PASSWORD"];

            if (string.IsNullOrEmpty(dbHost) || string.IsNullOrEmpty(dbDatabase) || string.IsNullOrEmpty(dbPassword))
            {
                _logger.LogError("Database configuration is incomplete. Please check the environment variables.");
                throw new InvalidOperationException("Database configuration is incomplete.");
            }

            return $"Host={dbHost};Port={dbPort};Database={dbDatabase};Username={dbUser};Password={dbPassword};";
        }
    }
}
