using ExchangeService.Application.DTO;
using ExchangeService.Domain.Interfaces.RepositoryInterfaces;
using ExchangeService.Domain.Models;
using System.Net.Http.Json;
using ExchangeService.Application.Service.Interfaces;
namespace ExchangeService.Application.Service
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IExchangeRateRepository _repository;
        private readonly HttpClient _httpClient;

        public ExchangeRateService(IExchangeRateRepository repository, HttpClient httpClient)
        {
            _repository = repository;
            _httpClient = httpClient;
        }

        public async Task<bool> ValidateAndLoadExchangeRatesAsync(DateTime date)
        {
            if (await _repository.ExchangeRatesExistAsync(date))
            {
                return true; 
            }

            var response = await _httpClient.GetAsync($"https://www.nbrb.by/api/exrates/rates?ondate={date:yyyy-MM-dd}&periodicity=0");
            if (response.IsSuccessStatusCode)
            {
                var rates = await response.Content.ReadFromJsonAsync<IEnumerable<ExchangeRateDTO>>();
                var exchangeRates = rates.Select(r => new ExchangeRate
                {
                    Date = date,
                    CurrencyCode = r.CurrencyCode,
                    Rate = r.Rate
                });

                await _repository.AddExchangeRatesAsync(exchangeRates);
                return true; 
            }

            return false; 
        }

        public async Task<ExchangeRate> GetExchangeRateAsync(DateTime date, string currencyCode)
        {
            return await _repository.GetExchangeRateByDateAndCodeAsync(date, currencyCode);
        }
    }
}
