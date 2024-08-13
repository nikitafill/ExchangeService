using System.Text.Json;
using AutoMapper;
using ExchangeService.Application.DTO;
using ExchangeService.Application.Service;
using ExchangeService.Application.Service.Interfaces;
using ExchangeService.Domain.Interfaces;
using ExchangeService.Domain.Interfaces.RepositoryInterfaces;
using ExchangeService.Domain.Models;
using Moq;
using Xunit;

namespace ExchangeService.Tests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly ExchangeRateService _service;
        private readonly Mock<IRepositoryManager> _repositoryManagerMock;
        private readonly Mock<IExchangeRateRepository> _exchangeRateRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Mock<IHttpClientService> _httpClientServiceMock;

        public ExchangeRateServiceTests()
        {
            _repositoryManagerMock = new Mock<IRepositoryManager>();
            _exchangeRateRepositoryMock = new Mock<IExchangeRateRepository>();
            _repositoryManagerMock.Setup(r => r.ExchangeRateRepository).Returns(_exchangeRateRepositoryMock.Object);

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Rate, RateDTO>();
            });
            _mapper = config.CreateMapper();

            _httpClientServiceMock = new Mock<IHttpClientService>();
            _service = new ExchangeRateService(_repositoryManagerMock.Object, _mapper, _httpClientServiceMock.Object);
        }

        [Fact]
        public async Task ValidateAndLoadExchangeRateAsync_ReturnsTrue_WhenRateIsLoadedAndSaved()
        {
            // Arrange
            var date = new DateTime(2023, 1, 10);
            var currencyCode = "USD";
            var rate = new Rate
            {
                Cur_ID = 431,
                Date = date,
                Cur_Abbreviation = "USD",
                Cur_Scale = 1,
                Cur_Name = "Доллар США",
                Cur_OfficialRate = 3.1277M
            };

            _exchangeRateRepositoryMock.Setup(r => r.GetExchangeRateByDateAndCodeAsync(date, currencyCode)).ReturnsAsync((Rate)null);
            _exchangeRateRepositoryMock.Setup(r => r.AddExchangeRateAsync(It.IsAny<Rate>())).Returns(Task.CompletedTask);

            _httpClientServiceMock.Setup(h => h.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(rate))
            });

            // Act
            var result = await _service.ValidateAndLoadExchangeRateAsync(date, currencyCode);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetExchangeRateByDateAndCodeAsync_ReturnsCorrectRateDTO()
        {
            // Arrange
            var date = new DateTime(2023, 1, 10);
            var currencyCode = "USD";
            var rate = new Rate
            {
                Cur_ID = 431,
                Date = date,
                Cur_Abbreviation = "USD",
                Cur_Scale = 1,
                Cur_Name = "Доллар США",
                Cur_OfficialRate = 3.1277M
            };

            _exchangeRateRepositoryMock.Setup(r => r.GetExchangeRateByDateAndCodeAsync(date, currencyCode)).ReturnsAsync(rate);

            // Act
            var result = await _service.GetExchangeRateByDateAndCodeAsync(date, currencyCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(rate.Cur_OfficialRate, result.Cur_OfficialRate);
        }

        [Fact]
        public async Task ValidateAndLoadExchangeRatesAsync_ReturnsTrue_WhenRatesAreLoadedAndSaved()
        {
            // Arrange
            var date = new DateTime(2023, 1, 10);
            var rates = new List<Rate>
        {
            new Rate {
                Cur_ID = 431,
                Date = date,
                Cur_Abbreviation = "USD",
                Cur_Scale = 1,
                Cur_Name = "Доллар США",
                Cur_OfficialRate = 3.1277M
            },
            new Rate {
                Cur_ID = 451,
                Date = date,
                Cur_Abbreviation = "EUR",
                Cur_Scale = 1,
                Cur_Name = "Евро",
                 Cur_OfficialRate = 3.3877M
            }
        };

            _exchangeRateRepositoryMock.Setup(r => r.ExchangeRatesExistAsync(date)).ReturnsAsync(false);
            _exchangeRateRepositoryMock.Setup(r => r.AddExchangeRatesAsync(It.IsAny<IEnumerable<Rate>>())).Returns(Task.CompletedTask);

            _httpClientServiceMock.Setup(h => h.GetAsync(It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(rates))
            });

            // Act
            var result = await _service.ValidateAndLoadExchangeRatesAsync(date);

            // Assert
            Assert.True(result);
        }
    }

}
