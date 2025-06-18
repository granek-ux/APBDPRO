using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Helpers;
using APBDPRO.Models;
using APBDPRO.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace APBDPROUnitTests;

public class ProfitServiceTests
{
    private readonly DatabaseContext _context;
    private readonly ProfitService _service;
    private readonly Mock<ICurrencyHelper> _currencyHelperMock;

    public ProfitServiceTests()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new DatabaseContext(options);

        _currencyHelperMock = new Mock<ICurrencyHelper>();
        _currencyHelperMock.Setup(x => x.ChangeCurrency(It.IsAny<double>(), It.IsAny<string>()))
            .ReturnsAsync((double amount, string _) => amount); // simulate no conversion

        _service = new ProfitService(_context, _currencyHelperMock.Object);

        SeedData();
    }

    private void SeedData()
    {
        var software = new Software
        {
            Id = 1,
            Name = "AppX",
            Description = "AppX Desc",
            ActualVersion = "v1.0.0",
            Price = 100,
            SoftwareCategoryId = 1,
            SoftwareCategory = new SoftwareCategory { Id = 1, Name = "Utility" }
        };

        var status = new StatusSubscription { Id = 1, Name = "Active" };

        var offer1 = new Offer
        {
            Id = 1,
            Price = 100,
            SoftwareId = 1,
            Software = software,
            ClientId = 1,
            Payments = new List<Payment>()
        };

        var offer2 = new Offer
        {
            Id = 2,
            Price = 200,
            SoftwareId = 1,
            Software = software,
            ClientId = 1,
            Payments = new List<Payment>()
        };

        var agreementSigned = new Agreement
        {
            OfferId = 1, 
            Offer = offer1,
            SoftwareVersion = "v1.0.0",
            YearsOfAssistance = 2,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(2),
            IsCanceled = false,
            IsSigned = true
        };

        var agreementUnsigned = new Agreement
        {
            OfferId = 2, // matches offer2
            Offer = offer2,
            SoftwareVersion = "v1.0.0",
            YearsOfAssistance = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            IsCanceled = false,
            IsSigned = false
        };

        var subscription = new Subscription
        {
            OfferId = 1,
            Offer = offer1,
            StatusSubscriptionId = 1,
            Name = "Standard"
        };

        var payment1 = new Payment
        {
            Id = 1,
            Offer = offer1,
            Amount = 100,
            Refunded = false
        };

        var payment2 = new Payment
        {
            Id = 2,
            Offer = offer2,
            Amount = 200,
            Refunded = false
        };

        offer1.Agreement = agreementSigned;
        offer2.Agreement = agreementUnsigned;
        offer1.Subscription = subscription;

        offer1.Payments.Add(payment1);
        offer2.Payments.Add(payment2);

        _context.Software.Add(software);
        _context.StatusSubsriptions.Add(status);
        _context.Offers.AddRange(offer1, offer2);
        _context.Agreements.AddRange(agreementSigned, agreementUnsigned);
        _context.Subscriptions.Add(subscription);
        _context.Payments.AddRange(payment1, payment2);

        _context.SaveChanges();
    }

    [Fact]
    public async Task GetAllProfit_ShouldReturnCorrectSum()
    {
        var result = await _service.GetAllProfit("PLN", CancellationToken.None);
        Assert.Equal(200, result); // jeśli payment2 ma podpisaną umowę
    }

    [Fact]
    public async Task GetProductProfit_ShouldReturnCorrectSum()
    {
        var result = await _service.GetProductProfit("PLN", "AppX", CancellationToken.None);
        Assert.Equal(300, result);
    }

    [Fact]
    public async Task GetAllProfitPredicted_ShouldIncludePredictedValues()
    {
        var result = await _service.GetAllProfitPredicted("PLN", CancellationToken.None);
        Assert.Equal(600, result);
    }

    [Fact]
    public async Task GetProductProfitPredicted_ShouldIncludePredictedValues()
    {
        var result = await _service.GetProductProfitPredicted("PLN", "AppX", CancellationToken.None);
        Assert.Equal(600, result);
    }

    [Fact]
    public async Task GetProductProfit_ShouldThrowIfSoftwareNotFound()
    {
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetProductProfit("PLN", "Nonexistent", CancellationToken.None));
    }

}