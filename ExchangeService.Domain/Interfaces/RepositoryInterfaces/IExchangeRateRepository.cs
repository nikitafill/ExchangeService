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
        Task<IEnumerable<Rate>> GetExchangeRatesByDateAsync(DateTime date);
        Task<Rate> GetExchangeRateByDateAndCodeAsync(DateTime date, string currencyCode);
        Task AddExchangeRatesAsync(IEnumerable<Rate> exchangeRates);
        Task<bool> ExchangeRatesExistAsync(DateTime date);
        Task AddExchangeRateAsync(Rate rate);
    }
}
