using ExchangeService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ExchangeService.Infrastructure.ContextFactory
{
    internal class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var buiilder = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseNpgsql(configuration.GetConnectionString("DbConnectionString"),
              b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));

            return new ApplicationDbContext(buiilder.Options);
        }
    }
}
