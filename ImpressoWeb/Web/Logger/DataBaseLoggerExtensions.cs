using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web.Logger
{
    public static class DataBaseLoggerExtensions
    {
        public static ILoggerFactory AddDataBase(this ILoggerFactory factory, IConfiguration configuration)
        {
            factory.AddProvider(new DataBaseLoggerProvider(configuration));
            return factory;
        }
    }
}