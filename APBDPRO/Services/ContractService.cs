using APBD25_CW11.Exceptions;
using APBDPRO.Data;
using APBDPRO.Models;
using APBDPRO.Models.Dtos;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace APBDPRO.Services;

public class ContractService : IContractService
{
    private readonly DatabaseContext _context;

    public ContractService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task AddAgreementAsync(AddAgreementDto addAgreementDto, CancellationToken cancellationToken)
    {
        if (addAgreementDto.EndDate - addAgreementDto.StartDate < TimeSpan.FromDays(3))
            throw new BadRequestException("Agreement must have at least 3 days");
        if (addAgreementDto.EndDate - addAgreementDto.StartDate > TimeSpan.FromDays(30))
            throw new BadRequestException("Agreement cannot have more than 30 days");

        if (addAgreementDto.HowMuchLongerAssistance > 3)
            throw new BadRequestException("Agreement assistance cannot be extended for more than 3 years");
        var client = addAgreementDto.PeselOrKrs.Length switch
        {
            11 => await _context.Clients.FirstOrDefaultAsync(e => e.Person.PESEL == addAgreementDto.PeselOrKrs,
                cancellationToken),
            10 => await _context.Clients.FirstOrDefaultAsync(e => e.Company.KRS == addAgreementDto.PeselOrKrs,
                cancellationToken),
            _ => throw new BadRequestException("Pesel Or Krs must have 11 or 10 days")
        };

        if (client is null)
            throw new NoMatchFoundException("Client not found");

        var software =
            await _context.Software.FirstOrDefaultAsync(e => e.Name == addAgreementDto.SoftwareName, cancellationToken);

        if (software is null)
            throw new NotFoundException("Software not found");

        var checkAgreement = await _context.Agreements.FirstOrDefaultAsync(
            e => e.ClientId == client.Id && e.SoftwareId == software.Id && e.EndDate >= DateTime.Today,
            cancellationToken);

        if (checkAgreement != null)
            throw new ConflictException("Agreement already exists");

        var checkPreviousAgreement =
            await _context.Agreements.AnyAsync(e => e.ClientId == client.Id, cancellationToken);

        //todo dorbić tu liczenie subskybji 
        var checkPreviousSubscription = false;

        var price = software.Price;

        price += addAgreementDto.HowMuchLongerAssistance * 1000;

        var discount = await _context.DiscountSoftware
            .Where(e => e.SoftwareId == software.Id && e.Discount.DateFrom <= DateTime.Today &&
                        e.Discount.DateTo >= DateTime.Today).DefaultIfEmpty()
            .MaxAsync(e => e.Discount.Value, cancellationToken);

        price = price * (1 - (double)discount / 100);

        if (checkPreviousAgreement || checkPreviousSubscription)
            price = price * 0.95;

        var agreement = new Agreement
        {
            ClientId = client.Id,
            SoftwareId = software.Id,
            SoftwareVersion = software.ActualVersion,
            YearsOfAssistance = 1 + addAgreementDto.HowMuchLongerAssistance,
            StartDate = addAgreementDto.StartDate,
            EndDate = addAgreementDto.EndDate,
            Price = price,
            IsPaid = false,
            IsSigned = false,
        };

        await _context.Agreements.AddAsync(agreement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task PayAgreementAsync(PayAgreementDto payAgreementDto, CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);


        try
        {
            var client = payAgreementDto.PeselOrKrs.Length switch
            {
                11 => await _context.Clients.FirstOrDefaultAsync(e => e.Person.PESEL == payAgreementDto.PeselOrKrs,
                    cancellationToken),
                10 => await _context.Clients.FirstOrDefaultAsync(e => e.Company.KRS == payAgreementDto.PeselOrKrs,
                    cancellationToken),
                _ => throw new BadRequestException("Pesel Or Krs must have 11 or 10 days")
            };

            if (client is null)
                throw new NoMatchFoundException("Client not found");

            var software =
                await _context.Software.FirstOrDefaultAsync(e => e.Name == payAgreementDto.SoftwareName,
                    cancellationToken);

            if (software is null)
                throw new NotFoundException("Software not found");

            var agreement = await _context
                .Agreements
                .Include(e => e.Payments)
                .FirstOrDefaultAsync(
                    e => e.ClientId == client.Id && e.SoftwareId == software.Id && e.IsPaid == false &&
                         e.EndDate >= DateTime.Today, cancellationToken);


            if (agreement is null)
                throw new NotFoundException("Agreement not found");

            var payedAgreement = agreement.Payments.Sum(e => e.Amount);

            var wantToPay = payAgreementDto.Amount;


            if (payedAgreement + wantToPay >= agreement.Price)
            {
                agreement.IsPaid = true;
                agreement.IsSigned = true;
                wantToPay = agreement.Price - payedAgreement;
            }

            var payment = new Payment
            {
                AgreementId = agreement.Id,
                Amount = wantToPay,
                PaymentDate = DateTime.Today,
            };

            await _context.AddAsync(payment, cancellationToken);


            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}