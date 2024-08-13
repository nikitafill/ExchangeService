using AutoMapper;
using ExchangeService.Application.DTO;
using ExchangeService.Application.Service.Interfaces;
using ExchangeService.Domain.Interfaces;
using ExchangeService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeService.Application.Service
{
    public class ExchangeRateService : IExchangeRateService
    {
        private static readonly string[] SupportedCurrencies = new[] { 
            "AUD", "AMD", "BGN", "BRL", "UAH", "DKK", "AED", "USD", "VND", "EUR", "PLN", "JPY", "INR", "IRR", "ISK", "CAD", 
            "CNY", "KWD", "MDL", "NZD", "NOK", "RUB", "XDR", "SGD", "KGS", "KZT", "TRY", "GBP", "CZK", "SEK", "CHF" 
        };

        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public ExchangeRateService(IRepositoryManager repositoryManager, IMapper mapper, HttpClient httpClient)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<RateDTO>> GetExchangeRatesByDateAsync(DateTime date)
        {
            var rates = await _repositoryManager.ExchangeRateRepository.GetExchangeRatesByDateAsync(date);
            return _mapper.Map<IEnumerable<RateDTO>>(rates);
        }

        public async Task<RateDTO> GetExchangeRateByDateAndCodeAsync(DateTime date, string currencyCode)
        {
            var rate = await _repositoryManager.ExchangeRateRepository.GetExchangeRateByDateAndCodeAsync(date, currencyCode);
            return _mapper.Map<RateDTO>(rate);
        }

        public async Task<bool> ValidateAndLoadExchangeRatesAsync(DateTime date)
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            if (await _repositoryManager.ExchangeRateRepository.ExchangeRatesExistAsync(date))
            {
                return true;
            }

            var rates = await FetchExchangeRatesFromNBRB(date);

            if (rates == null || rates.Count == 0)
            {
                return false;
            }

            await _repositoryManager.ExchangeRateRepository.AddExchangeRatesAsync(rates);

            return true;
        }
        public async Task<bool> ValidateAndLoadExchangeRateAsync(DateTime date, string currencyCode)
        {
            date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            if (!SupportedCurrencies.Contains(currencyCode))
            {
                return false;
            }

            var existingRate = await _repositoryManager.ExchangeRateRepository.GetExchangeRateByDateAndCodeAsync(date, currencyCode);

            if (existingRate != null)
            {
                return true;
            }

            var rate = await FetchExchangeRateFromNBRB(date, currencyCode);

            if (rate == null)
            {
                return false;
            }

            await _repositoryManager.ExchangeRateRepository.AddExchangeRateAsync(rate);

            return true;
        }

        private async Task<List<Rate>> FetchExchangeRatesFromNBRB(DateTime date)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            string url = $"https://www.nbrb.by/api/exrates/rates?ondate={formattedDate}&periodicity=0";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rates = JsonSerializer.Deserialize<List<Rate>>(content);

                return rates;
            }

            return null;
        }
        private async Task<Rate> FetchExchangeRateFromNBRB(DateTime date, string currencyCode)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            string url = $"https://www.nbrb.by/api/exrates/rates/{currencyCode}?ondate={formattedDate}&parammode=2";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rate = JsonSerializer.Deserialize<Rate>(content);

                return rate;
            }

            return null;
        }


    }
}
