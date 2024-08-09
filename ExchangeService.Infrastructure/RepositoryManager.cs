using ExchangeService.Infrastructure.DbContexts;
using ExchangeService.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeService.Infrastructure.Repositories;

namespace ExchangeService.Infrastructure
{
    public class RepositoryManager //: IRepositoryManager
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly Lazy<IExchangeRateRepository> _exchangeRateRepository;

        public RepositoryManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _exchangeRateRepository = new Lazy<IExchangeRateRepository>(() => new ExchangeRateRepository(_dbContext));
        }

        public IExchangeRateRepository ActorRepository => _exchangeRateRepository.Value;

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
