﻿using ExchangeService.Application.Service.Interfaces;
using ExchangeService.Application.Service;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ExchangeService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IExchangeRateService, ExchangeRateService>();
            return services;
        }
    }
}