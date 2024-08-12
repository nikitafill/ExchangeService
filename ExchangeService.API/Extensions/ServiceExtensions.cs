using ExchangeService.Infrastructure.DbContexts;
using ExchangeService.Domain.Models;
using ExchangeService.Infrastructure.DbContexts;
using System.Text;

namespace ExchangeService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
        }

        public static void ConfigureIIS(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => { });
        }

    }
}