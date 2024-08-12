using ExchangeService.API.Middleware;
using ExchangeService.Infrastructure;
using ExchangeService.Application;
using ExchangeService.API.Extensions;
using Serilog;
using ExchangeService.Application.Service.Interfaces;
using ExchangeService.Application.Service;
using ExchangeService.Domain.Interfaces.RepositoryInterfaces;
using ExchangeService.Infrastructure.Repositories;

namespace ExchangeService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((hostContext, services, configuration) => {
                configuration.MinimumLevel.Information();
                configuration.WriteTo.Console();
                configuration.WriteTo.File("logs/app.txt", rollingInterval: RollingInterval.Day);
            });

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddHttpClient();
            builder.Services.ConfigureIIS();
            builder.Services.ConfigureCors();

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            //app.ConfigureExceptionHandler(app.Logger);

            app.UseMiddleware<InitializeDatabaseMiddleware>(builder.Configuration);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}