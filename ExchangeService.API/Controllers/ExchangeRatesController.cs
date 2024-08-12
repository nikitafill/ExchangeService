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
            if (result)
            {
                return Ok("Data loaded or already exists.");
            }

            return BadRequest("Data loading failed.");
        }

        [HttpGet("{date}/{currencyCode}")]
        public async Task<IActionResult> GetExchangeRate(DateTime date, string currencyCode)
        {
            var rate = await _exchangeRateService.GetExchangeRateByDateAndCodeAsync(date, currencyCode);
            if (rate == null)
            {
                return NotFound();
            }

            return Ok(rate);
        }
    }
}
