using ExchangeService.Domain.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeService.Domain.Interfaces
{
    public interface IRepositoryManager
    {
        public IExchangeRateRepository ActorRepository { get; }

        public Task SaveAsync();
    }
}
