using ExchangeService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Application.Service.Interfaces
{
    public interface IExchangeRateService
    {
        Task<bool> ValidateAndLoadExchangeRatesAsync(DateTime date);
        Task<ExchangeRate> GetExchangeRateAsync(DateTime date, string currencyCode);
    }
}
