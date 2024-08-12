using ExchangeService.Application.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ExchangeService.API.Controllers
{
    [ApiController]
    [Route("api/exchangerates")]
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRatesController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet("validate/{date}")]
        public async Task<IActionResult> ValidateExchangeRates(DateTime date)
        {
            var result = await _exchangeRateService.ValidateAndLoadExchangeRatesAsync(date);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("{date}/{currencyCode}")]
        public async Task<IActionResult> GetExchangeRate(DateTime date, string currencyCode)
        {
            // Проверяем и загружаем данные для конкретной даты и валюты
            var result = await _exchangeRateService.ValidateAndLoadExchangeRateAsync(date, currencyCode);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
            // Получаем данные для конкретного кода валюты
            /*var rate = await _exchangeRateService.GetExchangeRateByDateAndCodeAsync(date, currencyCode);
            if (rate == null)
            {
                return NotFound("Exchange rate not found for the given date and currency code.");
            }

            return Ok(result);*/
        }


    }
}
