using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web.Logger
{
    public class DataBaseLoggerProvider : ILoggerProvider
    {
        private readonly IConfiguration _configuration;
        public DataBaseLoggerProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new DataBaseLogger(_configuration);
        }

        public void Dispose()
        {
        }
    }
}