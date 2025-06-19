using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using APBDPRO.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace APBDPROUnitTests;

public class SubscriptionsServiceTests : IDisposable
{
    private readonly SubscriptionsService _service;
    private readonly SqliteConnection _connection;
    private readonly DatabaseContext _context;

    public SubscriptionsServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new DatabaseContext(options);
        _context.Database.EnsureCreated();
        _service = new SubscriptionsService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task AddSubscriptionAsync_ShouldAddSubscription()
    {
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

        var softwareCategory = new SoftwareCategory
        {
            Name = "Testowa Kategoria"
        };

        var software = new Software
        {
            Name = "MyApp",
            ActualVersion = "1.0",
            Description = "Opis testowy",
            Price = 100,
            SoftwareCategory = softwareCategory
        };
        await _context.AddRangeAsync(person, client, softwareCategory, software);
        await _context.SaveChangesAsync();

        var dto = new AddSubscriptionDto
        {
            PeselOrKrs = "12345678901",
            SoftwareName = "MyApp",
            Name = "Sub1",
            Price = 200,
            RenewalPeriodDurationInMonths = 6
        };

        await _service.AddSubscriptionAsync(dto, CancellationToken.None);

        var subscription = await _context.Subscriptions.Include(s => s.Offer).FirstOrDefaultAsync(e => e.Name == dto.Name);
        Assert.NotNull(subscription);
        Assert.Equal(dto.Name, subscription.Name);
        Assert.Equal(client.Id, subscription.Offer.ClientId);
    }

    [Fact]
    public async Task AddSubscriptionAsync_ShouldThrow_WhenRenewalPeriodInvalid()
    {
        var dto = new AddSubscriptionDto
        {
            PeselOrKrs = "12345678901",
            SoftwareName = "MyApp",
            Name = "Sub1",
            Price = 200,
            RenewalPeriodDurationInMonths = 25
        };

        await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.AddSubscriptionAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task PaySubscriptionAsync_ShouldAddPayment()
    {
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

        var softwareCategory = new SoftwareCategory
        {
            Name = "Testowa Kategoria"
        };

        var software = new Software
        {
            Name = "MyApp",
            ActualVersion = "1.0",
            Description = "Opis testowy",
            Price = 100,
            SoftwareCategory = softwareCategory
        };
        var offer = new Offer { Client = client, Software = software, Price = 200};
        var subscription = new Subscription
        {
            Offer = offer,
            Name = "Sub1",
            PriceForFirstInstallment = 200,
            RenewalPeriodDurationInMonths = 1
        };

        await _context.AddRangeAsync(person, client, softwareCategory, software, offer, subscription);
        await _context.SaveChangesAsync();

        _context.Payments.Add(new Payment
        {
            Offer = offer,
            PaymentDate = DateTime.Today.AddMonths(-2),
            Amount = 200
        });
        await _context.SaveChangesAsync();

        var dto = new PaySubscriptionDto
        {
            SubscriptionName = "Sub1",
            Amount = 200
        };

        await _service.PaySubscriptionAsync(dto, CancellationToken.None);

        var payments = await _context.Payments.Where(p => p.OfferId == offer.Id).ToListAsync();
        Assert.Equal(2, payments.Count);
        Assert.Contains(payments, p => p.PaymentDate == DateTime.Today);
    }

    [Fact]
    public async Task PaySubscriptionAsync_ShouldThrow_WhenAlreadyPaid()
    {
        var offer = new Offer
        {
            Client = new Client
            {
                Person = new Person
                {
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    PESEL = "12345678901"
                },
                Adres = "ul. Testowa 1, 00-000 Warszawa",
                Email = "test@example.com",
                PhoneNumber = 123456789
            },
            Software = new Software
            {
                Name = "MyApp",
                ActualVersion = "1.0",
                Description = "Opis testowy",
                Price = 100,
                SoftwareCategory = new SoftwareCategory
                {
                    Name = "Testowa Kategoria"
                }
            },
            Price = 200
        };
        var subscription = new Subscription
        {
            Offer = offer,
            Name = "Sub1",
            RenewalPeriodDurationInMonths = 1
        };
        await _context.Offers.AddAsync(offer);
        await _context.SaveChangesAsync();
        await _context.Subscriptions.AddAsync(subscription);
        await _context.Payments.AddAsync(new Payment
        {
            Offer = offer,
            PaymentDate = DateTime.Today.AddDays(-10),
            Amount = 100,
            Refunded = false
        });
        await _context.SaveChangesAsync();

        var dto = new PaySubscriptionDto
        {
            SubscriptionName = "Sub1",
            Amount = 100
        };

        await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.PaySubscriptionAsync(dto, CancellationToken.None));
    }
}