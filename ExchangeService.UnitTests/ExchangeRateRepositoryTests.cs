using ExchangeService.Domain.Models;
using ExchangeService.Infrastructure.DbContexts;
using ExchangeService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ExchangeService.UnitTests
{
    public class ExchangeRateRepositoryTests
    {
        private readonly ExchangeRateRepository _repository;
        private readonly ApplicationDbContext _context;

        public ExchangeRateRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ExchangeRateTestDb")
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ExchangeRateRepository(_context);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetExchangeRatesByDateAsync_ReturnsRates_ForGivenDate()
        {
            // Arrange
            var date = new DateTime(2023, 1, 10);
            _context.Rates.AddRange(new List<Rate>
            {
                new Rate {
                    Cur_ID = 431,
                    Date = date,
                    Cur_Abbreviation = "USD",
                    Cur_Scale = 1,
                    Cur_Name = "Äמככאנ ÑØÀ",
                    Cur_OfficialRate = 3.1277M
                },
                new Rate {
                    Cur_ID = 451,
                    Date = date,
                    Cur_Abbreviation = "EUR",
                    Cur_Scale = 1,
                    Cur_Name = "Åגנמ",
                     Cur_OfficialRate = 3.3877M
                }
            });
            await _context.SaveChangesAsync();

            // Act
            var rates = await _repository.GetExchangeRatesByDateAsync(date);

            // Assert
            Assert.Equal(2, rates.Count());
        }

        [Fact]
        public async Task GetExchangeRateByDateAndCodeAsync_ReturnsCorrectRate_ForGivenDateAndCurrencyCode()
        {
            // Arrange
            var date = new DateTime(2023, 1, 10);
            var expectedRate = new Rate{
                Cur_ID = 431,
                Date = date,
                Cur_Abbreviation = "USD",
                Cur_Scale = 1,
                Cur_Name = "Äמככאנ ÑØÀ",
                Cur_OfficialRate = 3.1277M
            };
            _context.Rates.Add(expectedRate);
            await _context.SaveChangesAsync();

            // Act
            var rate = await _repository.GetExchangeRateByDateAndCodeAsync(date, "USD");

            // Assert
            Assert.NotNull(rate);
            Assert.Equal(expectedRate.Cur_OfficialRate, rate.Cur_OfficialRate);
        }

        [Fact]
        public async Task AddExchangeRateAsync_AddsRateToDatabase()
        {
            // Arrange
            var rate = new Rate
            {
                Cur_ID = 431,
                Date = new DateTime(2023, 1, 10),
                Cur_Abbreviation = "USD",
                Cur_Scale = 1,
                Cur_Name = "Äמככאנ ÑØÀ",
                Cur_OfficialRate = 3.1277M
            };
            // Act
            await _repository.AddExchangeRateAsync(rate);
            var savedRate = await _context.Rates.FirstOrDefaultAsync(r => r.Cur_Abbreviation == "USD");

            // Assert
            Assert.NotNull(savedRate);
            Assert.Equal(rate.Cur_OfficialRate, savedRate.Cur_OfficialRate);
        }

        [Fact]
        public async Task ExchangeRatesExistAsync_ReturnsTrue_WhenRatesExist()
        {
            // Arrange
            var date = new DateTime(2023, 1, 10);
            _context.Rates.Add(new Rate{
                Cur_ID = 431,
                Date = date,
                Cur_Abbreviation = "USD",
                Cur_Scale = 1,
                Cur_Name = "Äמככאנ ÑØÀ",
                Cur_OfficialRate = 3.1277M
            });
            await _context.SaveChangesAsync();

            // Act
            var exists = await _repository.ExchangeRatesExistAsync(date);

            // Assert
            Assert.True(exists);
        }
    }
}