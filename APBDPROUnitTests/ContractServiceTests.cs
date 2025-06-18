using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using APBDPRO.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APBDPROUnitTests
{
    public class ContractServiceTests : IDisposable
    {
        private readonly DatabaseContext _context;
        private readonly SqliteConnection _connection;
        private readonly ContractService _service;

        public ContractServiceTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new DatabaseContext(options);
            _context.Database.EnsureCreated();
            _service = new ContractService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Dispose();
        }

        [Fact]
        public async Task AddAgreementAsync_ShouldAddAgreement()
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
                Name = "TestSoft",
                ActualVersion = "1.0",
                Description = "Opis testowy",
                Price = 100,
                SoftwareCategory = softwareCategory
            };

            var offer = new Offer
            {
                Client = client,
                Software = software,
                Price = 999.99
            };

            await _context.People.AddAsync(person);
            await _context.Clients.AddAsync(client);
            await _context.Software.AddAsync(software);
            await _context.Offers.AddAsync(offer);
            await _context.SaveChangesAsync();

            var dto = new AddAgreementDto
            {
                PeselOrKrs = person.PESEL,
                SoftwareName = software.Name,
                StartDate = new DateTime(2025, 1, 1),
                EndDate = new DateTime(2025, 1, 25),
                HowMuchLongerAssistance = 2,
                Price = offer.Price
            };
            
            await _service.AddAgreementAsync(dto, CancellationToken.None);
            var agreement = await _context.Agreements.FirstOrDefaultAsync();
            Assert.NotNull(agreement);
            Assert.Equal(dto.StartDate, agreement.StartDate);
            Assert.Equal(dto.EndDate, agreement.EndDate);
            Assert.Equal(dto.HowMuchLongerAssistance + 1, agreement.YearsOfAssistance);
            Assert.Equal(dto.Price + dto.HowMuchLongerAssistance * 1000, agreement.Offer.Price);
            Assert.Equal(client.Id, agreement.Offer.ClientId);
            Assert.Equal(software.Id, agreement.Offer.SoftwareId);
        }

        [Fact]
        public async Task PayAgreementAsync_ShouldSignAgreement()
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
                Name = "TestSoft",
                ActualVersion = "1.0",
                Description = "Opis testowy",
                Price = 100,
                SoftwareCategory = softwareCategory
            };

            var offer = new Offer
            {
                Client = client,
                Software = software,
                Price = 500
            };

            var agreement = new Agreement
            {
                Offer = offer,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(10),
                SoftwareVersion = "1.0",
                YearsOfAssistance = 1,
                IsSigned = false,
                IsCanceled = false
            };
            
            await _context.People.AddAsync(person);
            await _context.Clients.AddAsync(client);
            await _context.SoftwareCategories.AddAsync(softwareCategory);
            await _context.Software.AddAsync(software);
            await _context.Offers.AddAsync(offer);
            await _context.Agreements.AddAsync(agreement);
            await _context.SaveChangesAsync();

            var dto = new PayAgreementDto
            {
                PeselOrKrs = "12345678901",
                SoftwareName = "TestSoft",
                Amount = 500
            };
            
            await _service.PayAgreementAsync(dto, CancellationToken.None);
            
            var updatedAgreement = await _context.Agreements
                .Include(a => a.Offer)
                .ThenInclude(o => o.Payments)
                .FirstOrDefaultAsync();

            Assert.NotNull(updatedAgreement);
            Assert.True(updatedAgreement.IsSigned);
            Assert.Single(updatedAgreement.Offer.Payments);
            Assert.Equal(500, updatedAgreement.Offer.Payments.First().Amount);
        }
    }
}