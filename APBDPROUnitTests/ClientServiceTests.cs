using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using APBDPRO.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace APBDPROUnitTests;

public class ClientServiceTests : IDisposable
{
    private readonly DatabaseContext _context;
    private readonly SqliteConnection _connection;
    private readonly ClientService _service;

    public ClientServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new DatabaseContext(options);
        _context.Database.EnsureCreated();

        _service = new ClientService(_context);
    }
    
    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task AddPersonAsync_ShouldAddPerson()
    {
        var dto = new PersonDto
        {
            PESEL = "12345678901",
            FirstName = "John",
            LastName = "Doe",
            Address = "Street 1",
            Email = "test@example.com",
            PhoneNumber = 123456789
        };

        await _service.AddPersonAsync(dto, CancellationToken.None);

        var person = await _context.People.FirstOrDefaultAsync(p => p.PESEL == dto.PESEL);
        Assert.NotNull(person);
        Assert.Equal("John", person.FirstName);
    }

    [Fact]
    public async Task AddPersonAsync_ShouldThrowConflict_WhenPersonExists()
    {
        var dto = new PersonDto
        {
            PESEL = "12345678901",
            FirstName = "John",
            LastName = "Doe",
            Address = "Street 1",
            Email = "test@example.com",
            PhoneNumber = 123456789
        };

        await _service.AddPersonAsync(dto, CancellationToken.None);

        await Assert.ThrowsAsync<ConflictException>(() => _service.AddPersonAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldSoftDelete()
    {
        var person = new Person
        {
            PESEL = "12345678901",
            FirstName = "John",
            LastName = "Doe",
            Deleted = false,
            Client = new Client { Adres = "Test", Email = "a", PhoneNumber = 1 }
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync();

        await _service.DeletePersonAsync("12345678901", CancellationToken.None);

        var updated = await _context.People.FirstAsync(p => p.PESEL == "12345678901");
        Assert.True(updated.Deleted);
    }

    [Fact]
    public async Task EditPersonAsync_ShouldUpdatePerson()
    {
        var client = new Client { Adres = "Old", Email = "old@mail.com", PhoneNumber = 1 };
        var person = new Person { PESEL = "12345678901", FirstName = "Old", LastName = "Name", Client = client };
        _context.People.Add(person);
        await _context.SaveChangesAsync();

        var editDto = new PersonEditDto
        {
            FirstName = "New",
            LastName = "Name",
            Address = "New Street",
            Email = "new@mail.com",
            PhoneNumber = 987654321
        };

        await _service.EditPersonAsync("12345678901", editDto, CancellationToken.None);

        var updated = await _context.People.Include(p => p.Client).FirstAsync(p => p.PESEL == "12345678901");
        Assert.Equal("New", updated.FirstName);
        Assert.Equal("New Street", updated.Client.Adres);
    }

    [Fact]
    public async Task AddCompanyAsync_ShouldAddCompany()
    {
        var dto = new CompanyDto
        {
            KRS = "1234567890",
            Name = "Test Co",
            Address = "Test St",
            Email = "co@example.com",
            PhoneNumber = 555111222
        };

        await _service.AddCompanyAsync(dto, CancellationToken.None);

        var company = await _context.Companies.FirstOrDefaultAsync(c => c.KRS == dto.KRS);
        Assert.NotNull(company);
        Assert.Equal("Test Co", company.Name);
    }

    [Fact]
    public async Task EditCompanyAsync_ShouldUpdateCompany()
    {
        var client = new Client { Adres = "Old", Email = "a", PhoneNumber = 1 };
        var company = new Company { KRS = "1234567890", Name = "OldName", Client = client };
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        var dto = new CompanyEditDto
        {
            Name = "NewName",
            Address = "NewAddress",
            Email = "new@mail.com",
            PhoneNumber = 999888777
        };

        await _service.EditCompanyAsync("1234567890", dto, CancellationToken.None);

        var updated = await _context.Companies.Include(c => c.Client).FirstAsync(c => c.KRS == "1234567890");
        Assert.Equal("NewName", updated.Name);
        Assert.Equal("NewAddress", updated.Client.Adres);
    }
}
