using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Helpers;
using APBDPRO.Models;
using APBDPRO.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace APBDPROUnitTests;

public class ProfitServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DatabaseContext _context;
    private readonly ProfitService _service;
    private readonly Mock<ICurrencyHelper> _currencyHelperMock;

    public ProfitServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new DatabaseContext(options);
        _context.Database.EnsureCreated();

        _currencyHelperMock = new Mock<ICurrencyHelper>();
        _currencyHelperMock.Setup(x => x.ChangeCurrency(It.IsAny<double>(), It.IsAny<string>()))
            .ReturnsAsync((double amount, string _) => amount);

        _service = new ProfitService(_context, _currencyHelperMock.Object);
        
        SeedDatabase().Wait();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
private async Task SeedDatabase()
        {
            var software = new Software
            {
                Name = "TestSoft",
                Description = "Opis",
                ActualVersion = "1.0",
                Price = 500,
                SoftwareCategory = new SoftwareCategory { Name = "TestCat" }
            };

            var person = new Person
            {
                FirstName = "Jan",
                LastName = "Kowalski",
                PESEL = "12345678901"
            };

            var client = new Client
            {
                Person = person,
                Adres = "ul. Testowa 1, 00-000 Warszawa",
                Email = "test@example.com",
                PhoneNumber = 123456789
            };

            var offer = new Offer
            {
                Client = client,
                Software = software,
                Price = 100
            };

            var subscription = new Subscription
            {
                Offer = offer,
                Name = "Sub1",
                PriceForFirstInstallment = 100,
                RenewalPeriodDurationInMonths = 1,
                StatusSubscription = new StatusSubscription
                {
                    Name = "Active"
                }
            };

            var agreement = new Agreement
            {
                Offer = offer,
                SoftwareVersion = "1.0",
                YearsOfAssistance = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                IsSigned = true,
                IsCanceled = false
            };

            var payment1 = new Payment
            {
                Offer = offer,
                Amount = 100,
                PaymentDate = DateTime.Today,
                Refunded = false
            };

            var payment2 = new Payment
            {
                Offer = offer,
                Amount = 200,
                PaymentDate = DateTime.Today,
                Refunded = false
            };

            await _context.AddAsync(subscription);
            await _context.AddAsync(agreement);
            await _context.AddAsync(payment1);
            await _context.AddAsync(payment2);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetProductProfit_ReturnsCorrectAmount()
        {
            var result = await _service.GetProductProfit("USD", "TestSoft", CancellationToken.None);

            Assert.Equal(600, result);
        }

        [Fact]
        public async Task GetProductProfitPredicted_ReturnsCorrectAmount()
        {
            var result = await _service.GetProductProfitPredicted("USD", "TestSoft", CancellationToken.None);

            Assert.Equal(600, result); 
        }

        [Fact]
        public async Task GetProductProfit_WithInvalidSoftware_ThrowsNotFoundException()
        {
            await Assert.ThrowsAsync<APBDPRO.Exceptions.NotFoundException>(() =>
                _service.GetProductProfit("USD", "InvalidSoft", CancellationToken.None));
        }

        [Fact]
        public async Task GetProductProfitPredicted_WithInvalidSoftware_ThrowsNotFoundException()
        {
            await Assert.ThrowsAsync<APBDPRO.Exceptions.NotFoundException>(() =>
                _service.GetProductProfitPredicted("USD", "InvalidSoft", CancellationToken.None));
        }
    }