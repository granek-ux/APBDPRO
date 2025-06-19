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
            
            _context.Agreements.RemoveRange(_context.Agreements);
            _context.Offers.RemoveRange(_context.Offers);
            await _context.SaveChangesAsync();

            var dto = new AddAgreementDto
            {
                PeselOrKrs = "90010112345", // seeded person
                SoftwareName = "SafeGuard", // seeded software
                StartDate = new DateTime(2025, 3, 15),
                EndDate = new DateTime(2025, 3, 25),
                HowMuchLongerAssistance = 2,
                Price = 49.99
            };

            await _service.AddAgreementAsync(dto, CancellationToken.None);

            var agreement = await _context.Agreements
                .Include(a => a.Offer)
                .FirstOrDefaultAsync();

            Assert.NotNull(agreement);
            Assert.Equal(dto.StartDate, agreement.StartDate);
            Assert.Equal(dto.EndDate, agreement.EndDate);
            Assert.Equal(dto.HowMuchLongerAssistance + 1, agreement.YearsOfAssistance);
            Assert.Equal("1.2.3", agreement.SoftwareVersion);
            Assert.Equal(2, agreement.Offer.ClientId);
            Assert.Equal(1, agreement.Offer.SoftwareId);
        }

        [Fact]
        public async Task PayAgreementAsync_ShouldSignAgreement()
        {
            var dto = new PayAgreementDto
            {
                PeselOrKrs = "90010112345",
                SoftwareName = "SafeGuard",
                Amount = 39.99
            };

            var existingAgreement = await _context.Agreements
                .Include(a => a.Offer)
                .ThenInclude(o => o.Payments)
                .FirstOrDefaultAsync(
                    e => e.Offer.ClientId == 2 && e.Offer.SoftwareId == 1 && e.IsSigned == false &&
                         e.EndDate >= DateTime.Today);

            if (existingAgreement is null)
            {
                var offer = new Offer { ClientId = 2, SoftwareId = 1, Price = 39.99 };
                var agreement = new Agreement
                {
                    Offer = offer,
                    StartDate = DateTime.Today.AddDays(-1),
                    EndDate = DateTime.Today.AddDays(10),
                    SoftwareVersion = "1.2.3",
                    YearsOfAssistance = 1,
                    IsSigned = false,
                    IsCanceled = false
                };

                await _context.Offers.AddAsync(offer);
                await _context.Agreements.AddAsync(agreement);
                await _context.SaveChangesAsync();
            }

            await _service.PayAgreementAsync(dto, CancellationToken.None);

            var updatedAgreement = await _context.Agreements
                .Include(a => a.Offer)
                .ThenInclude(o => o.Payments)
                .FirstOrDefaultAsync(a => a.Offer.ClientId == 1 && a.Offer.SoftwareId == 1);

            Assert.NotNull(updatedAgreement);
            Assert.True(updatedAgreement.IsSigned);
            Assert.Single(updatedAgreement.Offer.Payments);
            Assert.Equal(39.99, updatedAgreement.Offer.Payments.First().Amount);
        }
    }
}