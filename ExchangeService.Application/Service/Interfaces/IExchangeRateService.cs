using ExchangeService.Application.DTO;
using MediatR;
using System;

namespace ExchangeService.Application.Service.Interfaces
{
    public interface IExchangeRateService
    {
        Task<IEnumerable<RateDTO>> GetExchangeRatesByDateAsync(DateTime date);
        Task<RateDTO> GetExchangeRateByDateAndCodeAsync(DateTime date, string currencyCode);
        Task<bool> ValidateAndLoadExchangeRatesAsync(DateTime date);
        Task<bool> ValidateAndLoadExchangeRateAsync(DateTime date, string currencyCode);
    }
}
