using ExchangeService.Infrastructure.DbContexts;
using ExchangeService.Infrastructure.Repositories;
using ExchangeService.Domain.Interfaces;
using ExchangeService.Domain.Interfaces.RepositoryInterfaces;

namespace ExchangeService.Infrastructure
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly Lazy<IExchangeRateRepository> _exchangeRateRepository;

        public RepositoryManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _exchangeRateRepository = new Lazy<IExchangeRateRepository>(() => new ExchangeRateRepository(_dbContext));
        }

        public IExchangeRateRepository ActorRepository => _exchangeRateRepository.Value;

        IExchangeRateRepository IRepositoryManager.ActorRepository => throw new NotImplementedException();

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
