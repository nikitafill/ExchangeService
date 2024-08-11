using ExchangeService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain.Interfaces.RepositoryInterfaces
{
    public interface IExchangeRateRepository
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesByDateAsync(DateTime date);
        Task<ExchangeRate> GetExchangeRateByDateAndCodeAsync(DateTime date, string currencyCode);
        Task AddExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates);
        Task<bool> ExchangeRatesExistAsync(DateTime date);
    }
}
