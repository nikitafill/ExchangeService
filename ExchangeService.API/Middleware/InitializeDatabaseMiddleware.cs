using ExchangeService.Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.API.Middleware
{
    public class InitializeDatabaseMiddleware
    {
        private readonly RequestDelegate next;
        private static bool isInitialized = false;
        private readonly IConfiguration _configuration;

        private static object locker = new();

        public InitializeDatabaseMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            if (!isInitialized)
            {
                lock (locker)
                {
                    if (!isInitialized)
                    {
                        InitializeDatabase(serviceProvider);

                        isInitialized = true;
                    }
                }
            }

            await next.Invoke(context);
        }

        private void InitializeDatabase(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
        }
    }
}
