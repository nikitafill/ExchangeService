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

        [HttpGet("{date}/{Cur_Abbreviation}")]
        public async Task<IActionResult> GetExchangeRate(DateTime date, string currencyCode)
        {
            var result = await _exchangeRateService.ValidateAndLoadExchangeRateAsync(date, currencyCode);
            if (!result)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
