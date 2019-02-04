using System;
using Core;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Web.Logger
{
    public class DataBaseLogger : ILogger
    {
        private readonly object _lock = new object();
        private readonly IConfiguration _configuration;

        public DataBaseLogger(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null && exception != null)
            {
                lock (_lock)
                {
                    // TODO: Move it to serivces
                    DbContextOptionsBuilder<ImpressoDbContext> builder = new DbContextOptionsBuilder<ImpressoDbContext>();
                    builder.UseMySql(_configuration["Data:ConnectionString"]);
                    using (ImpressoDbContext context = new ImpressoDbContext(builder.Options))
                    {
                        context.Loggs.Add(new Log()
                        {
                            Message = formatter(state, exception),
                            ExceptionMessage = exception.Message,
                            InnerExceptionMessage = exception.InnerException is null ? "" : exception.InnerException.Message,
                            StackTrace = exception.StackTrace is null ? "" : exception.StackTrace,
                            Date = DateTime.Now
                        });
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}