using ExchangeService.Domain.Interfaces.RepositoryInterfaces;
using ExchangeService.Domain.Models;
using ExchangeService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Infrastructure.Repositories
{
    public class ExchangeRateRepository: IExchangeRateRepository
    {
        private readonly ApplicationDbContext _context;

        public ExchangeRateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesByDateAsync(DateTime date)
        {
            return await _context.ExchangeRates
                .Where(e => e.Date == date)
                .ToListAsync();
        }

        public async Task<ExchangeRate> GetExchangeRateByDateAndCodeAsync(DateTime date, string currencyCode)
        {
            return await _context.ExchangeRates
                .FirstOrDefaultAsync(e => e.Date == date && e.CurrencyCode == currencyCode);
        }

        public async Task AddExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates)
        {
            await _context.ExchangeRates.AddRangeAsync(exchangeRates);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExchangeRatesExistAsync(DateTime date)
        {
            return await _context.ExchangeRates.AnyAsync(e => e.Date == date);
        }
    }

}
