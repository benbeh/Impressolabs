using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Core
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ImpressoDbContext>
    {
        public ImpressoDbContext CreateDbContext(string[] args)
        {
            var conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var builder = new DbContextOptionsBuilder<ImpressoDbContext>();
            builder.UseMySql(conf["Data:ConnectionString"]);
            return new ImpressoDbContext(builder.Options);
        }
    }
}