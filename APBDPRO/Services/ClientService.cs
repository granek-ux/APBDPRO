using APBDPRO.Data;
using APBDPRO.Exceptions;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Services;

public class ClientService : IClientService
{
    private readonly DatabaseContext _context;

    public ClientService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddPersonAsync(PersonDto personDto, CancellationToken cancellationToken)
    {
        if (personDto.PESEL.Length != 11)
            throw new BadRequestException("PESEL must be 11 characters long");
        var person = await _context.Clients.Include(e => e.Person)
            .FirstOrDefaultAsync(e => e.Person.PESEL == personDto.PESEL && e.Person.Deleted == false,
                cancellationToken);
        if (person != null)
            throw new ConflictException("Person already exists");
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);


        try
        {
            var personToChange = await _context.Clients.Include(e => e.Person)
                .FirstOrDefaultAsync(e => e.Person.Deleted == true, cancellationToken);

            if (personToChange is null)
            {
                var client = new Client()
                {
                    Adres = personDto.Address,
                    Email = personDto.Email,
                    PhoneNumber = personDto.PhoneNumber,
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync(cancellationToken);
                var personToAdd = new Person()
                {
                    Id = client.Id,
                    PESEL = personDto.PESEL,
                    FirstName = personDto.FirstName,
                    LastName = personDto.LastName,
                    Client = client,
                };
                _context.People.Add(personToAdd);
            }
            else
            {
                personToChange.Adres = personDto.Address;
                personToChange.Email = personDto.Email;
                personToChange.PhoneNumber = personDto.PhoneNumber;

                personToChange.Person.PESEL = personDto.PESEL;
                personToChange.Person.FirstName = personDto.FirstName;
                personToChange.Person.LastName = personDto.LastName;
                personToChange.Person.Deleted = false;
            }

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return;
    }

    public async Task DeletePersonAsync(string pesel, CancellationToken cancellationToken)
    {
        if (pesel.Length != 11)
            throw new BadRequestException("PESEL must be 11 characters long");
        var person = await _context.Clients.Include(e => e.Person)
            .FirstOrDefaultAsync(e => e.Person.PESEL == pesel, cancellationToken);
        if (person is null || person.Person.Deleted)
            throw new NotFoundException($"Person with PESEL {pesel} not found");
        person.Person.Deleted = true;
        await _context.SaveChangesAsync(cancellationToken);
        return;
    }

    public async Task EditPersonAsync(string pesel, PersonEditDto personDto, CancellationToken cancellationToken)
    {
        if (pesel.Length != 11)
            throw new BadRequestException("PESEL must be 11 characters long");
        var person = await _context.People.Include(e => e.Client)
            .FirstOrDefaultAsync(e => e.PESEL == pesel, cancellationToken);
        if (person is null)
            throw new NotFoundException($"Person with PESEL {pesel} not found");

        if (personDto.FirstName != null && personDto.FirstName != person.FirstName)
            person.FirstName = personDto.FirstName;

        if (personDto.LastName != null && personDto.LastName != person.LastName)
            person.LastName = personDto.LastName;

        if (personDto.Address != null && personDto.Address != person.Client.Adres)
            person.Client.Adres = personDto.Address;

        if (personDto.Email != null && personDto.Email != person.Client.Email)
            person.Client.Email = personDto.Email;

        if (personDto.PhoneNumber != null && personDto.PhoneNumber != person.Client.PhoneNumber)
            person.Client.PhoneNumber = (int)personDto.PhoneNumber;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddCompanyAsync(CompanyDto companyDto, CancellationToken cancellationToken)
    {
        if (companyDto.KRS.Length != 10)
            throw new BadRequestException("KRS must be 10 characters long");

        var company = await _context.Clients.Include(e => e.Person)
            .FirstOrDefaultAsync(e => e.Company.KRS == companyDto.KRS, cancellationToken);
        if (company != null)
            throw new BadRequestException("Company already exists");
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var client = new Client()
            {
                Adres = companyDto.Address,
                Email = companyDto.Email,
                PhoneNumber = companyDto.PhoneNumber,
            };
            _context.Clients.Add(client);
            await _context.SaveChangesAsync(cancellationToken);
            var comapnyToAdd = new Company()
            {
                Id = client.Id,
                KRS = companyDto.KRS,
                Name = companyDto.Name,
                Client = client,
            };
            _context.Companies.Add(comapnyToAdd);
            await _context.SaveChangesAsync(cancellationToken);


            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        return;
    }

    public async Task EditCompanyAsync(string krs, CompanyEditDto companyDto, CancellationToken cancellationToken)
    {
        if (krs.Length != 10)
            throw new BadRequestException("KRS must be 10 characters long");

        var person = await _context.Companies.Include(e => e.Client)
            .FirstOrDefaultAsync(e => e.KRS == krs, cancellationToken);
        if (person is null)
            throw new NotFoundException($"Company with KRS {krs} not found");

        if (companyDto.Name != null && companyDto.Name != person.Name)
            person.Name = companyDto.Name;

        if (companyDto.Address != null && companyDto.Address != person.Client.Adres)
            person.Client.Adres = companyDto.Address;

        if (companyDto.Email != null && companyDto.Email != person.Client.Email)
            person.Client.Email = companyDto.Email;

        if (companyDto.PhoneNumber != null && companyDto.PhoneNumber != person.Client.PhoneNumber)
            person.Client.PhoneNumber = (int)companyDto.PhoneNumber;

        await _context.SaveChangesAsync(cancellationToken);
    }
}