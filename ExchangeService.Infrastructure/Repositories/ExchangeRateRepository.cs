using ExchangeService.Domain.Interfaces.RepositoryInterfaces;
using ExchangeService.Domain.Models;
using ExchangeService.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Infrastructure.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private readonly ApplicationDbContext _context;

        public ExchangeRateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rate>> GetExchangeRatesByDateAsync(DateTime date)
        {
            return await _context.Rates
                .Where(e => e.Date == date)
                .ToListAsync();
        }

        public async Task<Rate> GetExchangeRateByDateAndCodeAsync(DateTime date, string currencyCode)
        {
            return await _context.Rates
                .FirstOrDefaultAsync(e => e.Date == date && e.Cur_Abbreviation == currencyCode);
        }

        public async Task AddExchangeRatesAsync(IEnumerable<Rate> exchangeRates)
        {
            await _context.Rates.AddRangeAsync(exchangeRates);
            await _context.SaveChangesAsync();
        }
        public async Task AddExchangeRateAsync(Rate rate)
        {
            await _context.Rates.AddAsync(rate);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExchangeRatesExistAsync(DateTime date)
        {
            return await _context.Rates.AnyAsync(e => e.Date == date);
        }
    }

}
